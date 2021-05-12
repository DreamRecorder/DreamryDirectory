using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . General
{

	public class TargetEntityNotFoundException : TargetNotFoundException
	{

		public TargetEntityNotFoundException ( Guid targetGuid ) : base ( targetGuid ) { }

	}

}