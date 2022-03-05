using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic ;

public static class KnownLoginType
{

	[LoginType ( typeof ( PreSharedKeyCredential ) )]
	public static Guid PreSharedKey => Guid . Parse ( "58BD0A39-3F39-485F-9BC7-18642A48F85D" ) ;

}
