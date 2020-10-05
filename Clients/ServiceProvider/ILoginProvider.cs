using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface ILoginProvider
	{

		[CanBeNull]
		LoginToken Login(object credential);

		void CheckToken ( AccessToken token , LoginToken tokenToCheck ) ;

	}

}