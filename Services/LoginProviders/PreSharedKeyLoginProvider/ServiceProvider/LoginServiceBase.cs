using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Security . Cryptography ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Exceptions ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;
using DreamRecorder . Directory . Services . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider
{

	public abstract class LoginServiceBase <TCredential> : ILoginService
	{

		public abstract Guid Type { get ; }

		public abstract IDirectoryServiceProvider DirectoryServiceProvider { get ; }

		public IDirectoryService DirectoryService
			=> DirectoryServiceProvider . GetDirectoryService ( ) ;

		public abstract ITokenStorage <LoginToken> IssuedLoginTokens { get ; }

		public abstract IEntityTokenProvider EntityTokenProvider { get ; }

		public EntityToken EntityToken => EntityTokenProvider . GetToken ( ) ;

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

			if ( token . Target != EntityToken . Owner )
			{
				throw new InvalidTargetException ( ) ;
			}

			if ( ! DirectoryService . Contain (
												EntityToken ,
												KnownEntities . DirectoryServices ,
												token . Owner ) )
			{
				throw new InvalidOperationException ( ) ;
			}

			DirectoryService . CheckToken ( EntityToken , token ) ;

			tokenToCheck . CheckTokenTime ( ) ;

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
				DirectoryService . DisposeToken ( token ) ;
			}
		}

		public LoginToken IssueAccessToken ( [NotNull] Guid target )
		{
			DateTimeOffset now = DateTimeOffset . UtcNow ;

			TimeSpan lifetime = DirectoryService . GetLoginTokenLife ( EntityToken , target ) ;

			LoginToken token = new LoginToken
								{
									Owner     = target ,
									NotBefore = now ,
									NotAfter  = now + lifetime ,
									Issuer    = EntityTokenProvider . EntityGuid ,
									Secret    = RandomNumberGenerator . GetBytes ( 1024 ) ,
								} ;

			IssuedLoginTokens . AddToken ( token ) ;

			return token ;
		}

		public abstract Guid ? CheckCredential ( TCredential credential ) ;

	}

}
