using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . EntityFrameworkCore ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public interface IDirectoryDatabaseStorage
	{

		DbSet <DbUser> DbUsers { get ; set ; }

		DbSet <DbDirectoryService> DbDirectoryServices { get ; set ; }

		DbSet <DbLoginService> DbLoginServices { get ; set ; }

		DbSet <DbGroup> DbGroups { get ; set ; }

		DbSet <DbGroupMember> DbGroupMembers { get ; set ; }

		DbSet <DbProperty> DbProperties { get ; set ; }

		DbSet <DbPermissionGroup> DbPermissionGroups { get ; set ; }

		DbSet <DbService> DbServices { get ; set ; }

		void Save ( ) ;

	}

}
