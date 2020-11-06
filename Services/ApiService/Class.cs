using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DreamRecorder.Directory.Services.ApiService
{

	public class DirectoryDatabaseStorage : DbContext
	{

		public DbSet<DbUser> DbUsers { get; set; }
		public DbSet<DbDirectoryService> DbDirectoryServices { get; set; }
		public DbSet<DbLoginService> DbLoginServices { get; set; }
		public DbSet<DbGroup> DbGroups { get; set; }
		public DbSet<DbGroupMember> DbGroupMembers { get; set; }
		public DbSet<DbProperty> DbProperties { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder . Entity <DbUser> ( ) . HasKey ( user => user . Guid ) ;
			modelBuilder . Entity <DbDirectoryService> ( ) . HasKey ( directoryService => directoryService . Guid ) ;
			modelBuilder . Entity <DbLoginService> ( ) . HasKey ( loginService => loginService . Guid ) ;
			modelBuilder . Entity <DbGroup> ( ) . HasKey ( @group => @group . Guid ) ;

			modelBuilder . Entity <DbGroupMember> ( ) . HasKey ( (groupMember)=> new { groupMember.Group,groupMember.Member }) ;
			modelBuilder . Entity <DbProperty> ( ) . HasKey((property) => new { property.Owner, property.Name }) ;
		}

		public class DbUser
		{
			public Guid Guid { get; set; }

			public HashSet<DbProperty> Proprieties { get; set; }
		}

		public class DbDirectoryService
		{
			public Guid Guid { get; set; }

			public HashSet<DbProperty> Proprieties { get; set; }
		}

		public class DbLoginService
		{
			public Guid Guid { get; set; }

			public HashSet<DbProperty> Proprieties { get; set; }
		}


		public class DbGroup
		{
			public Guid Guid { get; set; }

			public HashSet<DbProperty> Proprieties { get; set; }

			public HashSet<DbGroupMember> Members { get; set; }

		}

		public class DbGroupMember
		{
			public Guid Group { get; set; }

			public Guid Member { get; set; }

		}

		public class DbProperty
		{
			public Guid Owner { get; set; }

			public string Name { get; set; }

			public string Value { get; set; }

			public string Permission { get; set; }
		}


	}

}
