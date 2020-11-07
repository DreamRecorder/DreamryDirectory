using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public interface ITokenPolicy
	{

		TimeSpan EntityTokenLife ( Entity entity ) ;

		TimeSpan AccessTokenLife ( Entity entity , Entity accessTarget ) ;

	}

}