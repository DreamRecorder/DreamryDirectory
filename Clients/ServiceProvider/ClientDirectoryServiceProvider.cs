using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class ClientDirectoryServiceProvider : IDirectoryServiceProvider
{

	public class StatefulRemoteDirectoryService
	{

		public enum RemoteState
		{

			Working,

			Disconnected,

			Checking,

		}

		public void Start()
		{
			TaskDispatcher.Dispatch(new ScheduledTask(UpdateStatus));
		}

		public DateTimeOffset? UpdateStatus()
		{
			if (Running)
			{
				lock (this)
				{
					try
					{
						if (State != RemoteState.Working)
						{
							State = RemoteState.Checking;
						}

						CurrentLatency += GetLatency().TotalMilliseconds;

						if (CurrentLatency < MinimumLatency)
						{
							MinimumLatency = CurrentLatency;
						}

						LatencySum        += CurrentLatency;
						LatencySquaredSum += CurrentLatency * CurrentLatency;

						LatencyCount++;

						AverageLatency = LatencySum / LatencyCount;
						LatencyVariance = (LatencySquaredSum / LatencyCount)
									- (AverageLatency        * AverageLatency);

						if (State != RemoteState.Working)
						{
							State        = RemoteState.Working;
							WorkingSince = DateTimeOffset.UtcNow;
						}
					}
					catch (Exception)
					{
						State = RemoteState.Disconnected;
					}
				}

				return DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10);
			}
			else
			{
				return null;
			}
		}

		public bool Running { get; private set; }

		public TaskDispatcher TaskDispatcher { get; }

		public DateTimeOffset WorkingSince { get; private set; }

		public RemoteState State { get; private set; } = RemoteState.Disconnected;

		public double CurrentLatency { get; private set; }

		public double AverageLatency { get; private set; }

		public double LatencySum { get; private set; }

		public double LatencyVariance { get; private set; }

		public double MinimumLatency { get; set; }

		public double LatencySquaredSum { get; private set; }

		public long LatencyCount { get; private set; } = 0;


		public TimeSpan GetLatency()
		{
			DateTimeOffset firstTime  = RemoteDirectoryService.GetTime();
			DateTimeOffset secondTime = RemoteDirectoryService.GetTime();

			return secondTime - firstTime;
		}

		public RemoteDirectoryService RemoteDirectoryService { get; }

		public StatefulRemoteDirectoryService(RemoteDirectoryService remoteDirectoryService)
		{
			RemoteDirectoryService = remoteDirectoryService;
		}

	}

	public IRandom Random { get; }

	public List<StatefulRemoteDirectoryService> KnownDirectoryService { get; }

	public ClientDirectoryServiceProvider(
		ICollection<(string HostName, int Port)> bootstrapDirectoryServers,
		IRandom                                  random)
	{
		Random = random;
		KnownDirectoryService =
			new List<StatefulRemoteDirectoryService>(
													bootstrapDirectoryServers.Select(
													(info)
														=> new
															StatefulRemoteDirectoryService(
															new
																RemoteDirectoryService(
																info))));

		foreach (StatefulRemoteDirectoryService remoteDirectoryService in
				KnownDirectoryService)
		{
			remoteDirectoryService.Start();
		}
	}

	public void UpdateServers()
	{
		lock (KnownDirectoryService)
		{
			IDirectoryService currentDirectoryService = GetDirectoryService();

			if (currentDirectoryService != null)
			{
				EntityToken anonymousToken =
					currentDirectoryService.Login(null)
				?? throw new InvalidOperationException();

				IEnumerable<(string HostName, int Port)> endpoints = currentDirectoryService.
					ListGroup(anonymousToken, KnownGroups.ServicingDirectoryServices).
					SelectMany(
								guid
									=> KnownProperties.ParseApiEndpoints(
									currentDirectoryService.GetProperty(
									anonymousToken,
									guid,
									KnownProperties.ApiEndpoints.
													GetPropertyName())));

				lock (KnownDirectoryService)
				{
					foreach ((string HostName, int Port) endpoint in endpoints)
					{
						if (!KnownDirectoryService.Any(
														sev
															=> sev.RemoteDirectoryService.
																	HostName == endpoint.HostName && sev.RemoteDirectoryService.Port == endpoint.Port))
						{
							KnownDirectoryService.Add(new StatefulRemoteDirectoryService(new RemoteDirectoryService(endpoint)));
						}
					}


				}
			}
		}
	}


	public IDirectoryService GetDirectoryService()
	{
		Comparer<StatefulRemoteDirectoryService> latencyComparer =
			Comparer<StatefulRemoteDirectoryService>.Create(
															ComparisonExtensions.
																Select<StatefulRemoteDirectoryService,
																	double>(
																(sev) => sev.CurrentLatency,
																ComparisonExtensions.IgnoreSmallDifference(0.5d)).
																Union(
																ComparisonExtensions.
																	Select<StatefulRemoteDirectoryService, double>(
																	(sev) => sev.AverageLatency,
																	ComparisonExtensions.IgnoreSmallDifference(0.5d))).
																Union(
																ComparisonExtensions.
																	Select<StatefulRemoteDirectoryService, double>(
																	(sev) => sev.LatencyVariance,
																	ComparisonExtensions.IgnoreSmallDifference(0.5d))).
																Union(
																ComparisonExtensions.
																	Select<StatefulRemoteDirectoryService, int>(
																	(sev) => Random.Next())));

		lock (KnownDirectoryService)
		{
			return KnownDirectoryService.
					Where(
						sev
							=> sev.State
							== StatefulRemoteDirectoryService.RemoteState.Working).
					Min(latencyComparer)?.
					RemoteDirectoryService;
		}
	}

}