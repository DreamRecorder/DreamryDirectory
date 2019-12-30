using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Entities ;

namespace DreamRecorder . Directory . Logic . Tokens
{

	public class AccessToken : Token
	{

		public Guid Target { get; set; }

	}

}