using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider ;

public class HashProvider : IHashProvider
{

	public IHash GetHash ( Guid guid ) => throw new NotImplementedException ( ) ;

}
