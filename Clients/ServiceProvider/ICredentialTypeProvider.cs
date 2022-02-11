using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . ServiceProvider ;

public interface ICredentialTypeProvider
{

	public Type GetCredentialType ( Guid loginType ) ;

}