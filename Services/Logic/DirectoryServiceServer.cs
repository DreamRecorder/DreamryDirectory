using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;
using DreamRecorder . Directory . Services . General ;
using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . ToolBox . General ;

using Microsoft . Extensions . DependencyInjection ;

namespace DreamRecorder . Directory . Services . Logic ;

public class DirectoryServiceServerConfig
{

	public Guid Guid { get ; set ; }

}

public class DirectoryServiceServer : DirectoryServiceBase
{

	public override IAccessTokenProvider AccessTokenProvider
		=> this.Lazy(() => ServiceProvider.GetService<IAccessTokenProvider>());

	public override IEntityTokenProvider EntityTokenProvider => this.Lazy(() => ServiceProvider.GetService<IEntityTokenProvider>());

	public override ILoginServiceProvider LoginServiceProvider => this.Lazy(() => ServiceProvider.GetService<ILoginServiceProvider>());

	public override IDirectoryServiceProvider DirectoryServiceProvider => this.Lazy(() => ServiceProvider.GetService<IDirectoryServiceProvider>());

	public override EntityToken AnonymousToken { get; }

	public override ITokenStorage<EntityToken> IssuedEntityTokens => this.Lazy(() => ServiceProvider.GetService<ITokenStorageFactory>()?.CreateTokenStorage<EntityToken>());

	public override ITokenStorage<AccessToken> IssuedAccessTokens => this.Lazy(() => ServiceProvider.GetService<ITokenStorageFactory>()?.CreateTokenStorage<AccessToken>());

	public override ITokenPolicy TokenPolicy => this.Lazy(() => ServiceProvider.GetService<ITokenPolicy>());

	public override IDirectoryDatabase DirectoryDatabase => this.Lazy(() => ServiceProvider.GetService<IDirectoryDatabase>());

	public override DirectoryService ServiceEntity { get; }

	public IServiceProvider ServiceProvider { get; }

	public DirectoryServiceServer(IServiceProvider serviceProvider, DirectoryServiceServerConfig config)
	{
		ServiceProvider = serviceProvider;

		ServiceEntity = new DirectoryService ( ) { Guid = config . Guid } ;
	}



	protected override void StartOverride()
	{
		(AccessTokenProvider as IStartStop)?.Start();
		(EntityTokenProvider as IStartStop)?.Start();
		(LoginServiceProvider as IStartStop)?.Start();
		(DirectoryServiceProvider as IStartStop)?.Start();
		(TokenPolicy as IStartStop)?.Start();
		(DirectoryDatabase as IStartStop)?.Start();
	}

}