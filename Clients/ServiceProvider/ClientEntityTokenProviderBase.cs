using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.ToolBox.General;

using JetBrains.Annotations;

namespace DreamRecorder.Directory.ServiceProvider;

public abstract class ClientEntityTokenProviderBase : IEntityTokenProvider, IStartStop
{

    protected IDirectoryServiceProvider DirectoryServiceProvider { get; }

    protected ITaskDispatcher TaskDispatcher { get; }

    protected abstract Func<LoginToken> GetLoginToken { get; }

    [CanBeNull]
    private EntityToken CurrentToken { get; set; }

    protected ClientEntityTokenProviderBase(
        IDirectoryServiceProvider directoryServiceProvider,
        ITaskDispatcher taskDispatcher)
    {
        DirectoryServiceProvider = directoryServiceProvider;
        TaskDispatcher = taskDispatcher;

    }

    public Guid EntityGuid => GetToken().Owner;

    public EntityToken GetToken()
    {
        lock (this)
        {
            while (true)
            {
                try
                {
                    CheckToken();
                    return CurrentToken;
                }
                catch (Exception)
                {
                    RenewToken();
                }

                Thread.Sleep(1);
            }
        }
    }

    public void CheckToken()
    {
        CurrentToken.CheckTokenTime();
        DirectoryServiceProvider.GetDirectoryService().CheckToken(CurrentToken);
    }

    public DateTimeOffset? RenewToken()
    {
        lock (this)
        {
            while (IsRunning)
            {
                try
                {
                    try
                    {
                        CurrentToken.CheckTokenTime();
                        CurrentToken = DirectoryServiceProvider.GetDirectoryService().
                            UpdateToken(CurrentToken);
                    }
                    catch (Exception)
                    {
                        CurrentToken = DirectoryServiceProvider.GetDirectoryService().
                            Login(GetLoginToken());
                    }

                    return DateTimeOffset.UtcNow
                        + ((CurrentToken.NotAfter - DateTimeOffset.UtcNow) / 2);
                }
                catch (Exception)
                {
                    Thread.Sleep(1);
                }
            }

            return default;
        }
    }

    public void Start()
    {
        lock (this)
        {
			if ( !IsRunning )
			{
				TaskDispatcher.Dispatch(new ScheduledTask(RenewToken));

				IsRunning = true;
            }
        }
    }

	public void Stop ( )
	{
		lock ( this )
		{
			IsRunning = false;
        }
    }

    public bool IsRunning { get; private set; }

}
