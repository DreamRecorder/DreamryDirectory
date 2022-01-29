using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class GlobalTokenPolicy : ITokenPolicy
	{

		public TimeSpan EntityTokenLife ( Entity entity ) => TimeSpan . FromMinutes ( 10 ) ;

		public TimeSpan AccessTokenLife ( Entity entity , Entity accessTarget )
			=> TimeSpan . FromMinutes ( 30 ) ;

		public TimeSpan LoginTokenLife ( Entity loginService , Entity entity )
			=> TimeSpan . FromMinutes ( 30 ) ;

	}

}
