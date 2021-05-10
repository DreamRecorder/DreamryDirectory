using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Storage ;

using Microsoft . EntityFrameworkCore ;

namespace DreamRecorder . Directory . Services . ApiService
{

	public class DirectoryDatabaseStorage : DbContext , IDirectoryDatabaseStorage
	{

		public DbSet <DbUser> DbUsers { get ; set ; }

		public DbSet <DbDirectoryService> DbDirectoryServices { get ; set ; }

		public DbSet <DbLoginService> DbLoginServices { get ; set ; }

		public DbSet <DbGroup> DbGroups { get ; set ; }

		public DbSet <DbGroupMember> DbGroupMembers { get ; set ; }

		public DbSet <DbProperty> DbProperties { get ; set ; }

		public DbSet <DbPermissionGroup> DbPermissionGroups { get ; set ; }

		public void Save ( ) { SaveChanges ( ) ; }

		public DbSet <DbService> DbServices { get ; set ; }

		protected override void OnConfiguring ( DbContextOptionsBuilder optionsBuilder )
		{
			optionsBuilder . UseSqlServer (
											"Data Source=sqlserver.dreamry.org;Initial Catalog=DreamryDirectory;Integrated Security=True" ) ;
		}

		protected override void OnModelCreating ( ModelBuilder modelBuilder )
		{
			modelBuilder . Entity <DbUser> ( ) . HasKey ( user => user . Guid ) ;
			modelBuilder . Entity <DbUser> ( ) .
							HasMany ( user => user . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Target ) ;

			modelBuilder . Entity <DbDirectoryService> ( ) . HasKey ( directoryService => directoryService . Guid ) ;
			modelBuilder . Entity <DbDirectoryService> ( ) .
							HasMany ( directoryService => directoryService . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Target ) ;

			modelBuilder . Entity <DbLoginService> ( ) . HasKey ( loginService => loginService . Guid ) ;
			modelBuilder . Entity <DbLoginService> ( ) .
							HasMany ( loginService => loginService . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Target ) ;

			modelBuilder . Entity <DbGroup> ( ) . HasKey ( group => group . Guid ) ;
			modelBuilder . Entity <DbGroup> ( ) .
							HasMany ( group => group . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Target ) ;
			modelBuilder . Entity <DbGroup> ( ) .
							HasMany ( group => group . Members ) .
							WithOne ( ) .
							HasForeignKey ( member => member . GroupGuid ) ;

			modelBuilder . Entity <DbService> ( ) . HasKey ( service => service . Guid ) ;
			modelBuilder . Entity <DbService> ( ) .
							HasMany ( service => service . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Target ) ;

			modelBuilder . Entity <DbPermissionGroup> ( ) . HasKey ( permissionGroup => permissionGroup . Guid ) ;

			modelBuilder . Entity <DbGroupMember> ( ) .
							HasKey (
									groupMember => new
													{
														Group  = groupMember . GroupGuid ,
														Member = groupMember . MemberGuid ,
													} ) ;
			modelBuilder . Entity <DbGroupMember> ( ) .
							HasOne ( groupMember => groupMember . Group ) .
							WithMany ( ) .
							HasForeignKey ( groupMember => groupMember . GroupGuid ) ;
			modelBuilder . Entity <DbProperty> ( ) .
							HasKey ( property => new { Owner = property . Target , property . Name , } ) ;
			modelBuilder . Entity <DbProperty> ( ) .
							HasOne ( prop => prop . Permission ) .
							WithMany ( ) .
							HasForeignKey ( prop => prop . PermissionGuid ) ;
		}

	}

}
