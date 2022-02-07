using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public interface ICredentialTypeProvider
{

	public Type GetCredentialType ( Guid loginType ) ;

}

public class CredentialTypeProvider : ICredentialTypeProvider
{

	private static Dictionary <Guid , Type> credentialTypes ;

	[Prepare]
	public static void Prepare ( )
	{
		lock ( StaticServiceProvider . ServiceCollection )
		{
			credentialTypes = 
			AppDomainExtensions .
				FindProperty <LoginTypeAttribute>(  (prop)=>prop.info.PropertyType == typeof(Guid)&&prop.info.GetAccessors().First().IsStatic ) .
				
				Select ( prop=>((Guid)prop.info.GetValue(null),prop.attribute.CredentialType)) .
				ToDictionary ( tuple => tuple . Item1,tuple=>tuple.CredentialType )  ;
		}
	}

	public Type GetCredentialType ( Guid loginType ) => credentialTypes [ loginType ] ;

}

public class ClientLoginServiceProvider : ILoginServiceProvider
{

	public List <StatefulRemoteService <RemoteLoginService>> KnownDirectoryService { get ; }

	public IDirectoryServiceProvider DirectoryServiceProvider { get ; }


	public ClientLoginServiceProvider ( IDirectoryServiceProvider directoryServiceProvider )
		=> DirectoryServiceProvider = directoryServiceProvider ;


	public ILoginService GetLoginService ( Guid type ) => throw new NotImplementedException ( ) ;

	public void UpdateServers ( )
	{
		lock ( KnownDirectoryService )
		{
			IDirectoryService currentDirectoryService =
				DirectoryServiceProvider . GetDirectoryService ( ) ;

			if ( currentDirectoryService != null )
			{
				EntityToken anonymousToken = currentDirectoryService . Login ( null )
										?? throw new InvalidOperationException ( ) ;

				IEnumerable <(string HostName , int Port)> endpoints = currentDirectoryService .
					ListGroup ( anonymousToken , KnownGroups . ServicingLoginServices ) .
					SelectMany (
								guid
									=> KnownProperties . ParseApiEndpoints (
									currentDirectoryService . GetProperty (
									anonymousToken ,
									guid ,
									KnownProperties . ApiEndpoints .
													GetPropertyName ( ) ) ) ) ;

				lock ( KnownDirectoryService )
				{
					foreach ( (string HostName , int Port) endpoint in endpoints )
					{
						if ( ! KnownDirectoryService . Any (
															sev
																=> sev . RemoteService . HostName
																== endpoint . HostName
																&& sev . RemoteService . Port
																== endpoint . Port ) )
						{
							KnownDirectoryService . Add (
														new
															StatefulRemoteService <
																RemoteLoginService> (
															new RemoteLoginService (
															endpoint ) ) ) ;
						}
					}
				}
			}
		}
	}

}

public class StatefulRemoteService <T> where T : RemoteServiceBase
{

	public enum RemoteState
	{

		Working ,

		Disconnected ,

		Checking ,

	}

	public bool Running { get ; private set ; }

	public TaskDispatcher TaskDispatcher { get ; }

	public DateTimeOffset WorkingSince { get ; private set ; }

	public RemoteState State { get ; private set ; } = RemoteState . Disconnected ;

	public double CurrentLatency { get ; private set ; }

	public double AverageLatency { get ; private set ; }

	public double LatencySum { get ; private set ; }

	public double LatencyVariance { get ; private set ; }

	public double MinimumLatency { get ; set ; }

	public double LatencySquaredSum { get ; private set ; }

	public long LatencyCount { get ; private set ; }


	public T RemoteService { get ; }

	public StatefulRemoteService ( T remoteService ) => RemoteService = remoteService ;

	public void Start ( ) { TaskDispatcher . Dispatch ( new ScheduledTask ( UpdateStatus ) ) ; }

	public DateTimeOffset ? UpdateStatus ( )
	{
		if ( Running )
		{
			lock ( this )
			{
				try
				{
					if ( State != RemoteState . Working )
					{
						State = RemoteState . Checking ;
					}

					CurrentLatency += RemoteService . MeasureLatency ( ) . TotalMilliseconds ;

					if ( CurrentLatency < MinimumLatency )
					{
						MinimumLatency = CurrentLatency ;
					}

					LatencySum        += CurrentLatency ;
					LatencySquaredSum += CurrentLatency * CurrentLatency ;

					LatencyCount++ ;

					AverageLatency = LatencySum / LatencyCount ;
					LatencyVariance = ( LatencySquaredSum / LatencyCount )
								- ( AverageLatency        * AverageLatency ) ;

					if ( State != RemoteState . Working )
					{
						State        = RemoteState . Working ;
						WorkingSince = DateTimeOffset . UtcNow ;
					}
				}
				catch ( Exception )
				{
					State = RemoteState . Disconnected ;
				}
			}

			return DateTimeOffset . UtcNow + TimeSpan . FromSeconds ( 10 ) ;
		}
		else
		{
			return null ;
		}
	}

}

public class ClientDirectoryServiceProvider : IDirectoryServiceProvider
{

	public IRandom Random { get ; }

	public List <StatefulRemoteService <RemoteDirectoryService>> KnownDirectoryService { get ; }

	public ClientDirectoryServiceProvider (
		ICollection <(string HostName , int Port)> bootstrapDirectoryServers ,
		IRandom                                    random )
	{
		Random = random ;
		KnownDirectoryService =
			new List <StatefulRemoteService <RemoteDirectoryService>> (
			bootstrapDirectoryServers . Select (
												info
													=> new
														StatefulRemoteService <
															RemoteDirectoryService> (
														new RemoteDirectoryService (
														info ) ) ) ) ;

		foreach ( StatefulRemoteService <RemoteDirectoryService> remoteDirectoryService in
				KnownDirectoryService )
		{
			remoteDirectoryService . Start ( ) ;
		}
	}


	public IDirectoryService GetDirectoryService ( )
	{
		Comparer <StatefulRemoteService <RemoteDirectoryService>> latencyComparer =
			Comparer <StatefulRemoteService <RemoteDirectoryService>> . Create (
			ComparisonExtensions .
				Select <StatefulRemoteService <RemoteDirectoryService> ,
					double> (
							sev => sev . CurrentLatency ,
							ComparisonExtensions . IgnoreSmallDifference ( 0.5d ) ) .
				Union (
						ComparisonExtensions .
							Select <StatefulRemoteService <RemoteDirectoryService> , double> (
							sev => sev . AverageLatency ,
							ComparisonExtensions . IgnoreSmallDifference ( 0.5d ) ) ) .
				Union (
						ComparisonExtensions .
							Select <StatefulRemoteService <RemoteDirectoryService> , double> (
							sev => sev . LatencyVariance ,
							ComparisonExtensions . IgnoreSmallDifference ( 0.5d ) ) ) .
				Union (
						ComparisonExtensions .
							Select <StatefulRemoteService <RemoteDirectoryService> , int> (
							sev => Random . Next ( ) ) ) ) ;

		lock ( KnownDirectoryService )
		{
			return KnownDirectoryService .
					Where (
							sev
								=> sev . State
								== StatefulRemoteService <RemoteDirectoryService> . RemoteState .
										Working ) .
					Min ( latencyComparer ) ? .
					RemoteService ;
		}
	}

	public void UpdateServers ( )
	{
		lock ( KnownDirectoryService )
		{
			IDirectoryService currentDirectoryService = GetDirectoryService ( ) ;

			if ( currentDirectoryService != null )
			{
				EntityToken anonymousToken = currentDirectoryService . Login ( null )
										?? throw new InvalidOperationException ( ) ;

				IEnumerable <(string HostName , int Port)> endpoints = currentDirectoryService .
					ListGroup ( anonymousToken , KnownGroups . ServicingDirectoryServices ) .
					SelectMany (
								guid
									=> KnownProperties . ParseApiEndpoints (
									currentDirectoryService . GetProperty (
									anonymousToken ,
									guid ,
									KnownProperties . ApiEndpoints .
													GetPropertyName ( ) ) ) ) ;

				lock ( KnownDirectoryService )
				{
					foreach ( (string HostName , int Port) endpoint in endpoints )
					{
						if ( ! KnownDirectoryService . Any (
															sev
																=> sev . RemoteService . HostName
																== endpoint . HostName
																&& sev . RemoteService . Port
																== endpoint . Port ) )
						{
							KnownDirectoryService . Add (
														new
															StatefulRemoteService <
																RemoteDirectoryService> (
															new RemoteDirectoryService (
															endpoint ) ) ) ;
						}
					}
				}
			}
		}
	}

}
