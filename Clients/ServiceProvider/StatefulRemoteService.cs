using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class StatefulRemoteService <T> : IStartStop where T : RemoteServiceBase
{

	public bool Running { get ; private set ; }

	public int Epoch { get ; set ; }

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

	public StatefulRemoteService ( T remoteService , int epoch )
	{
		RemoteService = remoteService ;
		Epoch         = epoch ;
	}

	public void Start ( ) { TaskDispatcher . Dispatch ( new ScheduledTask ( UpdateStatus ) ) ; }

	public void Stop ( ) { Running = false ; }

	public bool IsRunning { get ; private set ; }

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

					AverageLatency  = LatencySum / LatencyCount ;
					LatencyVariance = ( LatencySquaredSum / LatencyCount ) - ( AverageLatency * AverageLatency ) ;

					if ( Status != RemoteStatus . Working )
					{
						Status       = RemoteStatus . Working ;
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