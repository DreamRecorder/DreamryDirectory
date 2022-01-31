using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . Logic
{

	public interface ILoginService
	{
		LoginToken Login ( object credential ) ;

		void CheckToken ( AccessToken token , LoginToken tokenToCheck ) ;

		void DisposeToken ( LoginToken token ) ;

	}

}
