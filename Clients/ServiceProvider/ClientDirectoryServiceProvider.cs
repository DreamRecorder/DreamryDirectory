using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class ClientDirectoryServiceProvider
	: ClientServiceProviderBase <RemoteDirectoryService> , IDirectoryServiceProvider
{

	public ClientDirectoryServiceProvider (
		ICollection <(string HostName , int Port)> bootstrapDirectoryServers ,
		IRandom                                    random ,
		TaskDispatcher                             taskDispatcher ) : base ( random , taskDispatcher )
	{
		KnownRemoteService . AddRange (
										bootstrapDirectoryServers . Select (
																			info
																				=> new
																					StatefulRemoteService <
																						RemoteDirectoryService> (
																					new RemoteDirectoryService (
																					info ) ,
																					CurrentEpoch ) ) ) ;
	}


	public IDirectoryService GetDirectoryService ( ) => GetRemoteService ( ) ;

	public override void UpdateServers ( )
	{
		lock ( KnownRemoteService )
		{
			CurrentEpoch++ ;

			IDirectoryService currentDirectoryService = GetDirectoryService ( ) ;

			if ( currentDirectoryService != null )
			{
				EntityToken anonymousToken = currentDirectoryService . Login ( null )
											?? throw new InvalidOperationException ( ) ;

				IEnumerable <(string HostName , int Port)> endpoints = currentDirectoryService .
																		ListGroup (
																		anonymousToken ,
																		KnownGroups . ServicingDirectoryServices ) .
																		SelectMany (
																		guid
																			=> KnownProperties . ParseApiEndpoints (
																			currentDirectoryService .
																				GetProperty (
																				anonymousToken ,
																				guid ,
																				KnownProperties .
																					ApiEndpoints .
																					GetPropertyName ( ) ) ) ) ;

				lock ( KnownRemoteService )
				{
					foreach ( (string HostName , int Port) endpoint in endpoints )
					{
						if ( KnownRemoteService . FirstOrDefault (
																sev
																	=> sev . RemoteService . HostName
																		== endpoint . HostName
																		&& sev . RemoteService . Port
																		== endpoint . Port ) is
							StatefulRemoteService <RemoteDirectoryService> statefulService )
						{
							statefulService . Epoch = CurrentEpoch ;
						}

						StatefulRemoteService <RemoteDirectoryService> remoteService =
							new StatefulRemoteService <RemoteDirectoryService> (
																				new RemoteDirectoryService (
																				endpoint ) ,
																				CurrentEpoch ) ;

						remoteService . Start ( ) ;

						KnownRemoteService . Add ( remoteService ) ;
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

}
