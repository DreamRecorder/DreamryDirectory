using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic . Permissions
{

	public class Permission
	{

		public PermissionStatus Status { get; set; }

		public PermissionType Type { get; set; }

		public Entity Target { get; set; }

		public override string ToString ( )
		{
			return $"{Target.Guid},{Status},{Type}" ;
		}

		public Permission ( ) {
		}

		public Permission ( PermissionStatus status , PermissionType type , Entity target )
		{
			Status = status ;
			Type   = type ;
			Target = target ;
		}

	}

}