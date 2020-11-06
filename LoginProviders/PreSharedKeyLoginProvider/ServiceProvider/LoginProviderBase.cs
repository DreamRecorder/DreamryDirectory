using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider
{

	public abstract class LoginProviderBase<TCredential> : ILoginProvider
	{

		public abstract Guid ? CheckCredential ( TCredential credential ) ;

		public LoginToken Login(object credential)
		{
			throw null;
			if (credential is TCredential tCredential)
			{
				if ( CheckCredential(tCredential ) is Guid guid)
				{

				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		public void CheckToken(AccessToken token, LoginToken tokenToCheck) {; }


	}

}