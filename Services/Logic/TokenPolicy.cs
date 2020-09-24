using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class TokenPolicy : ITokenPolicy
	{

		public TimeSpan EntityTokenTimeSpan ( Entity entity ) { return TimeSpan . FromMinutes ( 10 ) ; }

		public TimeSpan AccessTokenLife ( Entity entity , Entity accessTarget )
		{
			return TimeSpan . FromMinutes ( 30 ) ;
		}

	}

}