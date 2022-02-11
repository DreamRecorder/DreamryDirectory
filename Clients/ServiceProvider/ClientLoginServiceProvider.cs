using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class ClientLoginServiceProvider : ClientServiceProviderBase <RemoteLoginService> , ILoginServiceProvider
{

	public IDirectoryServiceProvider DirectoryServiceProvider { get ; }

	public ICredentialTypeProvider CredentialTypeProvider { get ; }


	public ClientLoginServiceProvider (
		IDirectoryServiceProvider directoryServiceProvider ,
		IRandom                   random ,
		ICredentialTypeProvider   credentialTypeProvider ,
		TaskDispatcher            taskDispatcher ) : base ( random , taskDispatcher )
	{
		DirectoryServiceProvider = directoryServiceProvider ;
		CredentialTypeProvider   = credentialTypeProvider ;
	}

	public override void UpdateServers ( )
	{
		lock ( KnownRemoteService )
		{
			IDirectoryService directoryService = DirectoryServiceProvider . GetDirectoryService ( ) ;

			if ( directoryService != null )
			{
				EntityToken anonymousToken = directoryService . Login ( null )
											?? throw new InvalidOperationException ( ) ;

				IEnumerable <((string HostName , int Port) endpoint , Guid type)> endpoints = directoryService .
					ListGroup ( anonymousToken , KnownGroups . ServicingLoginServices ) .
					SelectMany (
								guid =>
								{
									Guid type = Guid . Parse (
															directoryService . GetProperty (
															anonymousToken ,
															guid ,
															KnownProperties . LoginType . GetPropertyName ( ) ) ) ;

									return ( KnownProperties . ParseApiEndpoints (
																directoryService . GetProperty (
																anonymousToken ,
																guid ,
																KnownProperties . ApiEndpoints .
																	GetPropertyName ( ) ) ) .
																Select ( endpoint => ( endpoint , type ) ) ) ;
								} ) ;

				lock ( KnownRemoteService )
				{
					foreach ( ((string HostName , int Port) endpoint , Guid type) endpointInfo in endpoints )
					{
						if ( KnownRemoteService . FirstOrDefault (
																sev
																	=> sev . RemoteService . HostName
																		== endpointInfo . endpoint . HostName
																		&& sev . RemoteService . Port
																		== endpointInfo . endpoint . Port
																		&& sev . RemoteService . Type
																		== endpointInfo . type ) is
							StatefulRemoteService <RemoteLoginService> statefulService )
						{
							statefulService . Epoch = CurrentEpoch ;
						}
						else
						{
							RemoteLoginService loginService =
								( RemoteLoginService )Activator . CreateInstance (
								typeof ( RemoteLoginService <> ) . MakeGenericType (
								CredentialTypeProvider . GetCredentialType ( endpointInfo . type ) ) ) ;

							StatefulRemoteService <RemoteLoginService> remoteService =
								new StatefulRemoteService <RemoteLoginService> ( loginService , CurrentEpoch ) ;
							remoteService . Start ( ) ;

							KnownRemoteService . Add ( remoteService ) ;
						}
					}

					KnownRemoteService . RemoveAll (
													sev =>
													{
														if ( sev . Epoch < CurrentEpoch )
														{
															sev . Stop ( ) ;
															return true ;
														}
														else
														{
															return false ;
														}
													} ) ;
				}
			}
		}
	}

	public override void StartOverride ( ) { UpdateServers ( ) ; }


	public ILoginService GetLoginService ( Guid type )
		=> GetRemoteService ( ( sev ) => sev . RemoteService . Type == type ) ;

}