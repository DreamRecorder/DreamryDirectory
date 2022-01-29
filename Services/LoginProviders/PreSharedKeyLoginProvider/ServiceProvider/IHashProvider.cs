using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider ;

public interface IHashProvider
{

	IHash GetHash ( Guid guid ) ;

}
