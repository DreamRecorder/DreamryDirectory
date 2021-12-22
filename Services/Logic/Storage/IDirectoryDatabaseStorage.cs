using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . EntityFrameworkCore ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public interface IDirectoryDatabaseStorage
	{

		DbSet <DbGroupMember> DbGroupMembers { get ; set ; }

		DbSet <DbProperty> DbProperties { get ; set ; }

		DbSet <DbPermissionGroup> DbPermissionGroups { get ; set ; }

		DbSet <DbEntity> DbEntities { get ; set ; }

		void Save ( ) ;

	}

}
