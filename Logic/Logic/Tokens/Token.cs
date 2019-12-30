using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic . Tokens
{

	public abstract class Token
	{

		public Guid Owner { get; set; }

		public byte[] Secret { get; set; }

		public DateTimeOffset NotBefore { get; set; }

		public DateTimeOffset NotAfter { get; set; }

		public Guid Issuer { get; set; }


	}

}