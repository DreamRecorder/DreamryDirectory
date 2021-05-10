using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . General
{

	public class TargetPropertyNotFoundException : TargetNotFoundException
	{

		public string TargetName { get ; set ; }

		public TargetPropertyNotFoundException ( Guid targetGuid , string targetName ) : base ( targetGuid )
			=> TargetName = targetName ;

	}

	public class TargetEntityNotFoundException : TargetNotFoundException
	{

		public TargetEntityNotFoundException ( Guid targetGuid ) : base ( targetGuid ) { }

	}

	public class TargetPermissionGroupNotFoundException : TargetNotFoundException
	{

		public TargetPermissionGroupNotFoundException ( Guid targetGuid ) : base ( targetGuid ) { }

	}

}
