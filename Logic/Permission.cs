using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace Logic
{

	public class Permission
	{

		public PermissionStatus Status { get; set; }

		public PermissionType Type { get; set; }

		public Entity Target { get; set; }

	}

}