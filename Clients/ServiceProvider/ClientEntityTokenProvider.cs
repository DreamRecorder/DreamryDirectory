using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class ClientEntityTokenProvider : ClientEntityTokenProviderBase
{

	private ILoginTokenProvider LoginTokenProvider { get ; }

	protected override Func <LoginToken> GetLoginToken => LoginTokenProvider . GetToken ;

	public ClientEntityTokenProvider (
		IDirectoryServiceProvider directoryServiceProvider ,
		ITaskDispatcher           taskDispatcher ,
		ILoginTokenProvider       loginTokenProvider ) : base (
																directoryServiceProvider ,
																taskDispatcher )
		=> LoginTokenProvider = loginTokenProvider ;

}
