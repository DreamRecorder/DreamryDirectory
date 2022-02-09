using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.ToolBox.General;

namespace DreamRecorder.Directory.Logic
{

    public interface ILoginService : IService
    {

        LoginToken Login(object credential);

        void CheckToken(AccessToken token, LoginToken tokenToCheck);

        void DisposeToken(LoginToken token);

    }

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
                if (StartupTime == null)
                {
                    StartupTime ??= DateTimeOffset.Now;
                }
            }
        }

        public void Stop()
        {
            lock (this)
            {
                StartupTime = null;
            }
        }

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

}
