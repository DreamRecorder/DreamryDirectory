using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic . Exceptions
{

	public class TargetPermissionGroupNotFoundException : TargetNotFoundException
	{

		public TargetPermissionGroupNotFoundException ( Guid targetGuid ) : base ( targetGuid ) { }

	}

}
