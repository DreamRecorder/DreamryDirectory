using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . Logic ;

public abstract class ServiceBase : IService, IStartStop
{

	public DateTimeOffset? StartupTime { get; private set; }

	public DateTimeOffset GetStartupTime() => StartupTime ?? DateTimeOffset.Now;

	public DateTimeOffset GetTime() => DateTime.Now;

	public virtual Version GetVersion() => GetType().Assembly.GetName().Version;

	public virtual void Start()
	{
		lock (this)
		{
			StartOverride();
			StartupTime ??= DateTimeOffset . Now ;
		}
	}

	protected abstract void StartOverride ( ) ;

	public void Stop()
	{
		lock (this)
		{
			StopOverride();
			StartupTime = null;
		}
	}

	protected virtual void StopOverride ( ) { }

	public bool IsRunning
	{
		get
		{
			lock (this)
			{
				return StartupTime != null;
			}
		}
	}

}
