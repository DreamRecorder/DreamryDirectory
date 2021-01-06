using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class GlobalTokenPolicy : ITokenPolicy
	{

		public TimeSpan EntityTokenLife ( Entity entity ) { return TimeSpan . FromMinutes ( 10 ) ; }

		public TimeSpan AccessTokenLife ( Entity entity , Entity accessTarget )
		{
			return TimeSpan . FromMinutes ( 30 ) ;
		}

		public TimeSpan LoginTokenLife ( Entity loginService , Entity entity )
		{
			return TimeSpan . FromMinutes ( 30 ) ;
		}

	}

}
