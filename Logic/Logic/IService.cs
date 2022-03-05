using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic ;

public interface IService
{

	DateTimeOffset GetStartupTime ( ) ;

	DateTimeOffset GetTime ( ) ;

	Version GetVersion ( ) ;

}