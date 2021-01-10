using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface IEntityTokenProvider
	{

		Guid EntityGuid { get ; }

		EntityToken GetToken ( ) ;

	}

}
