using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . Services . General ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider
{

	public abstract class LoginProviderBase <TCredential> : ILoginProvider
	{

		public TokenStorage <LoginToken> TokenStorage ;

		public LoginToken Login ( object credential )
		{
			if ( credential is TCredential tCredential )
			{
				if ( CheckCredential ( tCredential ) is Guid guid )
				{
				}
			}
			else
			{
				throw new InvalidOperationException ( ) ;
			}

			return default ; //todo
		}

		public void CheckToken ( AccessToken token , LoginToken tokenToCheck ) { ; }

		public abstract Guid ? CheckCredential ( TCredential credential ) ;

	}

}
