using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

using JetBrains . Annotations ;

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
			credentialTypes = AppDomainExtensions .
							FindProperty <LoginTypeAttribute> (
																( prop )
																	=> prop . info . PropertyType
																	== typeof ( Guid )
																	&& prop . info .
																			GetAccessors ( ) .
																			First ( ) .
																			IsStatic ) .
							Select (
									prop
										=> ( ( Guid )prop . info . GetValue ( null ) ,
											prop . attribute . CredentialType ) ) .
							ToDictionary (
										tuple => tuple . Item1 ,
										tuple => tuple . CredentialType ) ;
		}
	}

	public Type GetCredentialType ( Guid loginType ) => credentialTypes [ loginType ] ;

}

public abstract class ClientServiceProviderBase<TRemoteService> where TRemoteService:RemoteServiceBase
{

	public IRandom Random { get; }

	public List<StatefulRemoteService<TRemoteService>> KnownRemoteService { get; }=new List<StatefulRemoteService<TRemoteService>> ();

	public TaskDispatcher TaskDispatcher{ get; }

	public int CurrentEpoch { get; protected set; } = 0;

	protected ClientServiceProviderBase ( IRandom random , TaskDispatcher taskDispatcher )
	{
		Random         = random ;
		TaskDispatcher = taskDispatcher ;

		TaskDispatcher.Dispatch(new ScheduledTask())
	}



	protected virtual Predicate<StatefulRemoteService<TRemoteService>> WorkingFilter
		=> service => service . Status == RemoteStatus . Working ;

	public TRemoteService GetRemoteService ([CanBeNull] Predicate<StatefulRemoteService<TRemoteService>> requirement=null)
	{
		requirement ??= PredicateExtensions.Pass< StatefulRemoteService<TRemoteService>>() ;

		Comparer<StatefulRemoteService<TRemoteService>> latencyComparer =
			Comparer<StatefulRemoteService<TRemoteService>>.Create(
			ComparisonExtensions.
				Select<StatefulRemoteService<TRemoteService>,
					double>(
							sev => sev.CurrentLatency,
							ComparisonExtensions.IgnoreSmallDifference(0.5d)).
				Union(
					ComparisonExtensions.
						Select<StatefulRemoteService<TRemoteService>, double>(
						sev => sev.AverageLatency,
						ComparisonExtensions.IgnoreSmallDifference(0.5d))).
				Union(
					ComparisonExtensions.
						Select<StatefulRemoteService<TRemoteService>, double>(
						sev => sev.LatencyVariance,
						ComparisonExtensions.IgnoreSmallDifference(0.5d))).
				Union(
					ComparisonExtensions.
						Select<StatefulRemoteService<TRemoteService>, int>(
						sev => Random.Next())));

		lock (KnownRemoteService)
		{
			return KnownRemoteService.
					Where(PredicateExtensions.And(requirement,WorkingFilter).Invoke ).
					Min(latencyComparer) ?.
					RemoteService;
		}
	}

	public abstract void UpdateServers ( ) ;

}

public class ClientLoginServiceProvider : ClientServiceProviderBase<RemoteLoginService>, ILoginServiceProvider
{


	public IDirectoryServiceProvider DirectoryServiceProvider { get ; }

	public ICredentialTypeProvider CredentialTypeProvider { get ; }


	public ClientLoginServiceProvider (
		IDirectoryServiceProvider directoryServiceProvider ,
		IRandom                   random ,
		ICredentialTypeProvider   credentialTypeProvider ,
		TaskDispatcher            taskDispatcher ):base(random,taskDispatcher)
	{
		DirectoryServiceProvider = directoryServiceProvider ;
		CredentialTypeProvider   = credentialTypeProvider ;
	}

	

	public void UpdateServers ( )
	{
		lock ( KnownRemoteService )
		{
			IDirectoryService currentDirectoryService =
				DirectoryServiceProvider . GetDirectoryService ( ) ;

			if ( currentDirectoryService != null )
			{
				EntityToken anonymousToken = currentDirectoryService . Login ( null )
										?? throw new InvalidOperationException ( ) ;

				IEnumerable <((string HostName , int Port) endpoint , Guid type)> endpoints =
					currentDirectoryService .
						ListGroup ( anonymousToken , KnownGroups . ServicingLoginServices ) .
						SelectMany (
									guid =>
									{
										Guid type = Guid . Parse (
																currentDirectoryService .
																	GetProperty (
																	anonymousToken ,
																	guid ,
																	KnownProperties .
																		LoginType .
																		GetPropertyName ( ) ) ) ;

										return ( KnownProperties . ParseApiEndpoints (
														currentDirectoryService . GetProperty (
														anonymousToken ,
														guid ,
														KnownProperties . ApiEndpoints .
															GetPropertyName ( ) ) ) .
														Select (
																endpoint
																	=> ( endpoint , type ) ) ) ;
									} ) ;

				lock ( KnownRemoteService )
				{
					foreach ( ((string HostName , int Port) endpoint , Guid type) endpointInfo in
							endpoints )
					{
						if ( KnownRemoteService . FirstOrDefault (
													sev
														=> sev . RemoteService . HostName
														== endpointInfo . endpoint . HostName
														&& sev . RemoteService . Port
														== endpointInfo . endpoint . Port
														&& sev . RemoteService . Type
														== endpointInfo . type ) is StatefulRemoteService<RemoteLoginService> statefulService )
						{
							statefulService . Epoch = CurrentEpoch ;
						}

						RemoteLoginService remoteLogin =
							( RemoteLoginService )Activator . CreateInstance (
							typeof ( RemoteLoginService <> ) . MakeGenericType (
							CredentialTypeProvider . GetCredentialType (
							endpointInfo . type ) ) ) ;

						KnownRemoteService . Add (
												new StatefulRemoteService <RemoteLoginService> (
												remoteLogin ) ) ;
					}
				}
			}
		}
	}

	public ILoginService GetLoginService ( Guid type ) => GetRemoteService((sev)=>sev.RemoteService.Type==type) ;

}

public enum RemoteStatus
{

	Working,

	Disconnected,

	Checking,

}

public class StatefulRemoteService <T> :IStartStop where T : RemoteServiceBase
{

	public bool Running { get ; private set ; }

	public int Epoch{ get; set; }

	public TaskDispatcher TaskDispatcher { get ; }

	public DateTimeOffset WorkingSince { get ; private set ; }

	public RemoteStatus Status { get ; private set ; } = RemoteStatus . Disconnected ;

	public double CurrentLatency { get ; private set ; }

	public double AverageLatency { get ; private set ; }

	public double LatencySum { get ; private set ; }

	public double LatencyVariance { get ; private set ; }

	public double MinimumLatency { get ; set ; }

	public double LatencySquaredSum { get ; private set ; }

	public long LatencyCount { get ; private set ; }


	public T RemoteService { get ; }

	public StatefulRemoteService ( T remoteService ) => RemoteService = remoteService ;

	public void Start ( )
	{
		TaskDispatcher . Dispatch ( new ScheduledTask ( UpdateStatus ) ) ;
	}

	public void Stop(){ Running = false; }

	public bool IsRunning { get; private set; }

	public DateTimeOffset ? UpdateStatus ( )
	{
		if ( Running )
		{
			lock ( this )
			{
				try
				{
					if ( Status != RemoteStatus . Working )
					{
						Status = RemoteStatus . Checking ;
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

					if ( Status != RemoteStatus . Working )
					{
						Status        = RemoteStatus . Working ;
						WorkingSince = DateTimeOffset . UtcNow ;
					}
				}
				catch ( Exception )
				{
					Status = RemoteStatus . Disconnected ;
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

public class ClientDirectoryServiceProvider : ClientServiceProviderBase<RemoteDirectoryService>, IDirectoryServiceProvider
{

	public ClientDirectoryServiceProvider (
		ICollection <(string HostName , int Port)> bootstrapDirectoryServers ,
		IRandom                                    random ,
		TaskDispatcher                             taskDispatcher ):base(random,taskDispatcher)
	{
		KnownRemoteService .AddRange(
			bootstrapDirectoryServers . Select (
												info
													=> new
														StatefulRemoteService <
															RemoteDirectoryService> (
														new RemoteDirectoryService (
														info ) ) ) ) ;

		foreach ( StatefulRemoteService <RemoteDirectoryService> remoteDirectoryService in
				KnownRemoteService )
		{
			remoteDirectoryService . Start ( ) ;
		}
	}


	public IDirectoryService GetDirectoryService ( ) =>GetRemoteService ( ) ;

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
					ListGroup ( anonymousToken , KnownGroups . ServicingDirectoryServices ) .
					SelectMany (
								guid
									=> KnownProperties . ParseApiEndpoints (
									currentDirectoryService . GetProperty (
									anonymousToken ,
									guid ,
									KnownProperties . ApiEndpoints .
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
															== endpoint . Port ) is StatefulRemoteService<RemoteDirectoryService> statefulService)
						{
							statefulService . Epoch = CurrentEpoch ;
						}

						StatefulRemoteService <RemoteDirectoryService> remoteService =
							new StatefulRemoteService <RemoteDirectoryService> (
							new RemoteDirectoryService ( endpoint ) ) { Epoch = CurrentEpoch } ;

						remoteService . Start ( ) ;

						KnownRemoteService . Add (remoteService ) ;
					}

					KnownRemoteService . RemoveAll ( sev =>
														{
															if (sev.Epoch < CurrentEpoch)
															{
																sev.Stop();
																return true;
															}
															else
															{
																return false;
															}
														} ) ;
				}
			}
		}
	}

}
