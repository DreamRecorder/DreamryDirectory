using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace Logic
{

	public class Token
	{

		public Guid Entity { get; set; }

		public Guid Secret { get; set; }

		public DateTime NotBefore { get; set; }

		public DateTime NotAfter { get; set; }

	}

}