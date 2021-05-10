using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic
{

	public class PermissionGroup
	{

		public Guid Guid { get ; set ; }

		public Guid Owner { get ; set ; }

		public HashSet <Permission> Permissions { get ; set ; } = new HashSet <Permission> ( ) ;

	}

}
