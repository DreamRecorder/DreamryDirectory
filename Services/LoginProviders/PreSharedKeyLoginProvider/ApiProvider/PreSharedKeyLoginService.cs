using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Security . Cryptography ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . LoginProviders . ServiceProvider ;
using DreamRecorder . Directory . ServiceProvider ;
using DreamRecorder . Directory . Services . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ApiService
{

	public class PreSharedKeyLoginService : LoginServiceBase <PreSharedKeyCredential>
	{

		public IHashProvider HashProvider { get ; }

		public override Guid Type => KnownLoginType . PreSharedKey ;

		public override IDirectoryServiceProvider DirectoryServiceProvider { get ; }

		public override ITokenStorage <LoginToken> IssuedLoginTokens { get ; }

		public override IEntityTokenProvider EntityTokenProvider { get ; }

		public PreSharedKeyLoginService (
			IEntityTokenProvider       entityTokenProvider ,
			IHashProvider              hashProvider ,
			IDirectoryServiceProvider  directoryServiceProvider ,
			ITokenStorage <LoginToken> issuedLoginTokens )
		{
			EntityTokenProvider      = entityTokenProvider ;
			HashProvider             = hashProvider ;
			DirectoryServiceProvider = directoryServiceProvider ;
			IssuedLoginTokens        = issuedLoginTokens ;
		}

		public override Guid ? CheckCredential ( [NotNull] PreSharedKeyCredential credential )
		{
			if ( credential == null )
			{
				throw new ArgumentNullException ( nameof ( credential ) ) ;
			}

			if ( DirectoryService . GetProperty (
												EntityToken ,
												credential . Target ,
												Constants . EnabledPropertyName ) is string enabled
			&& Convert . ToBoolean ( enabled ) )
			{
				if ( DirectoryService . GetProperty (
													EntityToken ,
													credential . Target ,
													Constants . HashPropertyName ) is string
					hashString )
				{
					byte [ ] hash = Convert . FromBase64String ( hashString ) ;

					if ( DirectoryService . GetProperty (
														EntityToken ,
														credential . Target ,
														Constants . SaltPropertyName ) is string
						saltString )
					{
						if ( DirectoryService . GetProperty (
															EntityToken ,
															credential . Target ,
															Constants . HashVersion ) is string
							hashVersionString )
						{
							if ( HashProvider . GetHash ( Guid . Parse ( hashVersionString ) ) is
								IHash hashFunc )
							{
								if ( CryptographicOperations . FixedTimeEquals (
									hashFunc . Hash ( credential . PreSharedKey , saltString ) ,
									hash ) )
								{
									return credential . Target ;
								}
							}
						}
					}
				}
			}

			return null ;
		}

		protected override void StartOverride ( ) { throw new NotImplementedException ( ) ; }

	}

}
