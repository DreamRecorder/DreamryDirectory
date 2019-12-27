using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . Logic
{

	public interface ILoginProvider
	{

		LoginToken Login(object credential);

		bool CheckToken ( EntityToken token , LoginToken tokenToCheck ) ;

	}

}