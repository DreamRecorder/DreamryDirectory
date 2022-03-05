using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic ;

public interface ILoginServiceProvider
{

	ILoginService GetLoginService ( Guid type ) ;

}