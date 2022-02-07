using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic ;

[Flags]
public enum EntityScope : byte
{

	Anonymous = 1 << 0 ,

	User = 1 << 1 ,

	DirectoryService = 1 << 2 ,

	LoginService = 1 << 3 ,

	Group = 1 << 4 ,

	Service = 1 << 5 ,

	SpecialGroup = 1 << 6 ,

	Any = byte . MaxValue ,

}
