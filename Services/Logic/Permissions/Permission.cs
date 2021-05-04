using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder.Directory.Services.General;
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

		public DreamRecorder . Directory . Logic . Permission ToClientSidePermission ( )
		{
			return new Directory . Logic . Permission ( Status , Type , Target.Guid ) ;
		}

		public static Permission Create (DreamRecorder.Directory.Logic.Permission permission )
		{
			Entity target = DirectoryServiceInternal . Current . DirectoryDatabase . FindEntity ( permission . Target ) ;

			if ( target!=null )
			{
				Permission result = new Permission ( )
									{
										Status = permission . Status , Target = target , Type = permission . Type ,
									} ;

				return result;
			}
			else
			{
				throw new TargetEntityNotFoundException (permission.Target ) ;
			}

		}
	}

}
