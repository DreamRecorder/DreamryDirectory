using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . ToolBox . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . ServiceProvider ;

public abstract class ClientServiceProviderBase <TRemoteService> : IStartStop where TRemoteService : RemoteServiceBase
{

	public IRandom Random { get ; }

	public List <StatefulRemoteService <TRemoteService>> KnownRemoteService { get ; } =
		new List <StatefulRemoteService <TRemoteService>> ( ) ;

	public TaskDispatcher TaskDispatcher { get ; }

	public int CurrentEpoch { get ; protected set ; } = 0 ;

	protected ClientServiceProviderBase ( IRandom random , TaskDispatcher taskDispatcher )
	{
		Random         = random ;
		TaskDispatcher = taskDispatcher ;
	}

	protected virtual Predicate <StatefulRemoteService <TRemoteService>> WorkingFilter
		=> service => service . Status == RemoteStatus . Working ;

	public TRemoteService GetRemoteService (
		[CanBeNull] Predicate <StatefulRemoteService <TRemoteService>> requirement = null )
	{
		requirement ??= PredicateExtensions . Pass <StatefulRemoteService <TRemoteService>> ( ) ;

		Comparer <StatefulRemoteService <TRemoteService>> latencyComparer =
			Comparer <StatefulRemoteService <TRemoteService>> . Create (
																		ComparisonExtensions .
																			Select <
																				StatefulRemoteService <TRemoteService> ,
																				double> (
																			sev => sev . CurrentLatency ,
																			ComparisonExtensions .
																				IgnoreSmallDifference ( 0.5d ) ) .
																			Union (
																			ComparisonExtensions .
																				Select <
																					StatefulRemoteService <
																						TRemoteService> , double> (
																				sev => sev . AverageLatency ,
																				ComparisonExtensions .
																					IgnoreSmallDifference (
																					0.5d ) ) ) .
																			Union (
																			ComparisonExtensions .
																				Select <
																					StatefulRemoteService <
																						TRemoteService> , double> (
																				sev => sev . LatencyVariance ,
																				ComparisonExtensions .
																					IgnoreSmallDifference (
																					0.5d ) ) ) .
																			Union (
																			ComparisonExtensions .
																				Select <
																					StatefulRemoteService <
																						TRemoteService> , int> (
																				sev => Random . Next ( ) ) ) ) ;

		lock ( KnownRemoteService )
		{
			return KnownRemoteService . Where ( PredicateExtensions . And ( requirement , WorkingFilter ) . Invoke ) .
										Min ( latencyComparer ) ? .
										RemoteService ;
		}
	}

	public abstract void UpdateServers ( ) ;

	public DateTimeOffset ? UpdateServerTask ( )
	{
		if ( IsRunning )
		{
			UpdateServers ( ) ;
			return DateTimeOffset . UtcNow + TimeSpan . FromHours ( 1 ) ;
		}

		return default ;
	}

	public virtual void StartOverride ( ) { }

	public void Start ( )
	{
		lock ( this )
		{
			if ( ! IsRunning )
			{
				StartOverride ( ) ;
				TaskDispatcher . Dispatch ( new ScheduledTask ( UpdateServerTask ) ) ;
				foreach ( StatefulRemoteService <TRemoteService> statefulRemoteService in KnownRemoteService )
				{
					statefulRemoteService . Start ( ) ;
				}

				IsRunning = true ;
			}
		}
	}

	public void Stop ( )
	{
		lock ( this )
		{
			StopOverride ( ) ;
			foreach ( StatefulRemoteService <TRemoteService> statefulRemoteService in KnownRemoteService )
			{
				statefulRemoteService . Stop ( ) ;
			}

			IsRunning = false ;
		}
	}

	public virtual void StopOverride ( ) { }


	public bool IsRunning { get ; private set ; }

}