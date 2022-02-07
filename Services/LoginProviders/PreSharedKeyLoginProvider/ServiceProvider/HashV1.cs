using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . AspNetCore . Cryptography . KeyDerivation ;

namespace DreamRecorder . Directory . LoginProviders . ServiceProvider ;

public class HashV1 : IHash
{

	public Guid Guid => Guid . Parse ( "3DEAC6C3-4516-4990-B2D1-C8C02EE4D214" ) ;

	public byte [ ] Hash ( string password , string salt )
		=> KeyDerivation . Pbkdf2 (
									password ,
									Convert . FromBase64String ( salt ) ,
									KeyDerivationPrf . HMACSHA512 ,
									120000 ,
									512 / 8 ) ;

}
