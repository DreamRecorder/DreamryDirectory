using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Security . Cryptography ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;
using DreamRecorder . Directory . Services . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider
{

	public abstract class LoginServiceBase <TCredential> : ILoginService
	{

		public virtual IDirectoryServiceProvider DirectoryService { get ; }

		public virtual TokenStorage <LoginToken> IssuedLoginTokens { get ; }

		public virtual IEntityTokenProvider EntityTokenProvider { get ; }

		public virtual RNGCryptoServiceProvider RngProvider { get ; set ; } = new RNGCryptoServiceProvider ( ) ;

		public LoginToken Login ( object credential )
		{
			if ( credential is TCredential tCredential )
			{
				if ( CheckCredential ( tCredential ) is Guid guid )
				{
					return IssueAccessToken ( guid ) ;
				}
				else
				{
					return null ;
				}
			}
			else
			{
				throw new InvalidOperationException ( ) ;
			}
		}

		public void CheckToken ( AccessToken token , LoginToken tokenToCheck )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( tokenToCheck == null )
			{
				throw new ArgumentNullException ( nameof ( tokenToCheck ) ) ;
			}

			DirectoryService . GetDirectoryService ( ) . CheckToken ( EntityTokenProvider . GetToken ( ) , token ) ;

			token . CheckTokenTime ( ) ;

			if ( token . Issuer == EntityTokenProvider . EntityGuid )
			{
				IssuedLoginTokens . CheckToken ( tokenToCheck ) ;
			}
			else
			{
				throw new CannotCheckByIssuerException ( ) ;
			}
		}

		public void DisposeToken ( LoginToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( token . Issuer == EntityTokenProvider . EntityGuid )
			{
				IssuedLoginTokens . DisposeToken ( token ) ;
			}
			else
			{
				DirectoryService . GetDirectoryService ( ) . DisposeToken ( token ) ;
			}
		}

		public LoginToken IssueAccessToken ( [NotNull] Guid target )
		{
			DateTimeOffset now = DateTimeOffset . UtcNow ;

			TimeSpan lifetime = DirectoryService . GetDirectoryService ( ) .
													GetLoginTokenLife ( EntityTokenProvider . GetToken ( ) , target ) ;

			LoginToken token = new LoginToken
								{
									Owner     = target ,
									NotBefore = now ,
									NotAfter  = now + lifetime ,
									Issuer    = EntityTokenProvider . EntityGuid ,
									Secret    = new byte[ 1024 ] ,
								} ;

			RngProvider . GetBytes ( token . Secret ) ;

			IssuedLoginTokens . AddToken ( token ) ;

			return token ;
		}

		public abstract Guid ? CheckCredential ( TCredential credential ) ;

	}

}
