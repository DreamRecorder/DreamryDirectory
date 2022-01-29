using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;

namespace DreamRecorder . Directory . Services . Logic
{

	public interface IDirectoryDatabase
	{

		KnownSpecialGroups KnownSpecialGroups { get ; set ; }

		PermissionGroup FindPermissionGroup ( Guid guid ) ;

		Entity FindEntity ( Guid guid ) ;

		EntityProperty FindProperty ( Guid entity , string name ) ;

		Entity AddEntity ( Entity entity ) ;

		PermissionGroup AddPermissionGroup ( PermissionGroup permissionGroup ) ;

		void AddEntityProperty ( EntityProperty property ) ;

		void Save ( ) ;

		void Initiate ( ) ;

		void CreateNew ( ) ;

	}

}
