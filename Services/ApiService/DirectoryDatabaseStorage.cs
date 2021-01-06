using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Storage ;

using JetBrains . Annotations ;

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


		public HashSet <DbUser> GetDbUsers ( ) { return new HashSet <DbUser> ( DbUsers ) ; }

		public HashSet <DbDirectoryService> GetDbDirectoryServices ( )
		{
			return new HashSet <DbDirectoryService> ( DbDirectoryServices ) ;
		}

		public HashSet <DbLoginService> GetDbLoginServices ( )
		{
			return new HashSet <DbLoginService> ( DbLoginServices ) ;
		}

		public HashSet <DbGroup> GetDbGroups ( ) { return new HashSet <DbGroup> ( DbGroups ) ; }

		public HashSet <DbGroupMember> GetDbGroupMembers ( ) { return new HashSet <DbGroupMember> ( DbGroupMembers ) ; }

		public HashSet <DbProperty> GetDbProperties ( ) { return new HashSet <DbProperty> ( DbProperties ) ; }

		public void Save ( ) { SaveChanges ( ) ; }

		public void DeleteGroupMember ( [NotNull] DbGroupMember groupMember )
		{
			if ( groupMember == null )
			{
				throw new ArgumentNullException ( nameof ( groupMember ) ) ;
			}

			DbGroupMembers . Remove ( groupMember ) ;
			Save ( ) ;
		}

		public void DeleteProperty ( [NotNull] DbProperty property )
		{
			if ( property == null )
			{
				throw new ArgumentNullException ( nameof ( property ) ) ;
			}

			DbProperties . Remove ( property ) ;
			Save ( ) ;
		}

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
							HasForeignKey ( prop => prop . Owner ) ;

			modelBuilder . Entity <DbDirectoryService> ( ) . HasKey ( directoryService => directoryService . Guid ) ;
			modelBuilder . Entity <DbDirectoryService> ( ) .
							HasMany ( directoryService => directoryService . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Owner ) ;

			modelBuilder . Entity <DbLoginService> ( ) . HasKey ( loginService => loginService . Guid ) ;
			modelBuilder . Entity <DbLoginService> ( ) .
							HasMany ( loginService => loginService . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Owner ) ;

			modelBuilder . Entity <DbGroup> ( ) . HasKey ( group => group . Guid ) ;
			modelBuilder . Entity <DbGroup> ( ) .
							HasMany ( group => group . Proprieties ) .
							WithOne ( ) .
							HasForeignKey ( prop => prop . Owner ) ;
			modelBuilder . Entity <DbGroup> ( ) .
							HasMany ( group => group . Members ) .
							WithOne ( ) .
							HasForeignKey ( member => member . Group ) ;

			modelBuilder . Entity <DbGroupMember> ( ) .
							HasKey ( groupMember => new { groupMember . Group , groupMember . Member } ) ;
			modelBuilder . Entity <DbProperty> ( ) . HasKey ( property => new { property . Owner , property . Name } ) ;
		}

	}

}
