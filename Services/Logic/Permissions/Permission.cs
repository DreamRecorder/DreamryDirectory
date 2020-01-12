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

	}

}