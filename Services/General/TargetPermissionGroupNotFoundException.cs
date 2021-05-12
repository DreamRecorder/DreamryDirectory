using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . General
{

	public class TargetPermissionGroupNotFoundException : TargetNotFoundException
	{

		public TargetPermissionGroupNotFoundException ( Guid targetGuid ) : base ( targetGuid ) { }

	}

}