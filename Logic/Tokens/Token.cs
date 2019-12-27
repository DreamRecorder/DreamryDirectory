using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic . Tokens
{

	public abstract class Token
	{

		public Guid Owner { get; set; }

		public Guid Secret { get; set; }

		public DateTime NotBefore { get; set; }

		public DateTime NotAfter { get; set; }

	}

}