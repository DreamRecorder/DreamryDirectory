using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic . Permissions
{

	public class Permission
	{

		public PermissionStatus Status { get ; set ; }

		public PermissionType Type { get ; set ; }

		public Entity Target { get ; set ; }

		public Permission ( ) { }


		public Permission ( Entity target , PermissionStatus status , PermissionType type )
		{
			Target = target ;
			Status = status ;
			Type   = type ;
		}

		public override string ToString ( ) { return $"{Target . Guid},{Status},{Type}" ; }

	}

}
