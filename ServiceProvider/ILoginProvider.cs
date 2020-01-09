using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder.Directory.ServiceProvider.Tokens ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface ILoginProvider
	{

		LoginToken Login(object credential);

		bool CheckToken ( AccessToken token , LoginToken tokenToCheck ) ;

	}

}