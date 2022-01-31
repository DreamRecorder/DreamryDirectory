using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System . Threading ;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.ToolBox.General;

using JetBrains.Annotations;

namespace DreamRecorder.Directory.ServiceProvider
{

    public interface IEntityTokenProvider
    {

        Guid EntityGuid { get; }

        EntityToken GetToken();

    }

    public interface ILoginTokenProvider
    {

        LoginToken GetToken();

    }

	public class ClientDirectoryServiceProvider : IDirectoryServiceProvider
	{

		public List <(string HostName,int Port)> KnowDirectoryService { get ; }

		public ClientDirectoryServiceProvider (ICollection<(string HostName, int Port)> bootstrapDirectoryServers)
		{
			KnowDirectoryService = new List <(string HostName, int Port)> (bootstrapDirectoryServers ) ;
		}

		public void UpdateServers ( )
		{
			


			RemoteDirectoryService remoteDirectoryService
		}


		public IDirectoryService GetDirectoryService ( ) => throw new NotImplementedException ( ) ;

	}



    public class ClientLoginTokenProvider : ILoginTokenProvider
	{

		public ICollection<(Guid loginProvider, object credential)> Credentials { get; }


        public LoginToken GetToken ( ) => throw new NotImplementedException ( ) ;

	}

	public class AnonymousEntityTokenProvider : ClientEntityTokenProviderBase
	{

		public AnonymousEntityTokenProvider ( IDirectoryServiceProvider directoryServiceProvider , ITaskDispatcher taskDispatcher ) : base ( directoryServiceProvider , taskDispatcher )
		{
		}

		protected override Func<LoginToken> GetLoginToken => () => null;

	}


	public abstract class ClientEntityTokenProviderBase : IEntityTokenProvider
	{

		protected ClientEntityTokenProviderBase ( IDirectoryServiceProvider directoryServiceProvider , ITaskDispatcher taskDispatcher )
		{
			DirectoryServiceProvider = directoryServiceProvider ;
			TaskDispatcher           = taskDispatcher ;

			TaskDispatcher.Dispatch(new ScheduledTask(RenewToken));
		}

		protected IDirectoryServiceProvider DirectoryServiceProvider { get; }

		protected ITaskDispatcher TaskDispatcher { get; }

		protected abstract Func<LoginToken> GetLoginToken{ get; }

		public Guid EntityGuid => GetToken().Owner;

		[CanBeNull]
		private EntityToken CurrentToken { get; set; } = null;

		public void CheckToken()
		{
			CurrentToken.CheckTokenTime();
			DirectoryServiceProvider.GetDirectoryService().CheckToken(CurrentToken);
		}

		public DateTimeOffset RenewToken()
		{
			lock (this)
			{
				while (true)
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
						Thread . Sleep ( 1 ) ;
					}

				}

			}

		}

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

	}

	public class ClientEntityTokenProvider : ClientEntityTokenProviderBase
	{
		

		private ILoginTokenProvider LoginTokenProvider { get; }

		public ClientEntityTokenProvider ( IDirectoryServiceProvider directoryServiceProvider , ITaskDispatcher taskDispatcher , ILoginTokenProvider loginTokenProvider ) : base ( directoryServiceProvider , taskDispatcher )
		{
			LoginTokenProvider = loginTokenProvider ;
		}

		protected override Func <LoginToken> GetLoginToken => LoginTokenProvider . GetToken ;

	}

}
