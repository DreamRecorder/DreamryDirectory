using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class AnonymousEntityTokenProvider : ClientEntityTokenProviderBase
{

	public AnonymousEntityTokenProvider(
		IDirectoryServiceProvider directoryServiceProvider,
		ITaskDispatcher           taskDispatcher) : base(directoryServiceProvider, taskDispatcher)
	{
	}

	protected override Func<LoginToken> GetLoginToken => () => null;

}