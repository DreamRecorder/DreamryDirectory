using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text . Json ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;
using DreamRecorder . Directory . Services . Logic . Storage ;

using Microsoft . EntityFrameworkCore ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class DirectoryDatabase : IDirectoryDatabase
	{

		public IDirectoryDatabaseStorage DatabaseStorage { get ; set ; }

		public IDirectoryServiceInternal DirectoryServiceInternal { get ; set ; }

		public IEnumerable <Entity> Entities
			=> Users . Union <Entity> ( Groups ) .
						Union ( Services ) .
						Union ( LoginServices ) .
						Union ( DirectoryServices ) .
						Union ( KnownSpecialGroups . Entities ) .
						Union ( new [ ] { Anonymous , } ) ;

		public Anonymous Anonymous { get ; set ; } = new Anonymous ( ) ;

		public DirectoryDatabase ( IDirectoryDatabaseStorage databaseStorage ) => DatabaseStorage = databaseStorage ;

		public HashSet <User> Users { get ; set ; }

		public HashSet <Group> Groups { get ; set ; }

		public HashSet <Service> Services { get ; set ; }

		public HashSet <LoginService> LoginServices { get ; set ; }

		public HashSet <DirectoryService> DirectoryServices { get ; set ; }

		public HashSet <PermissionGroup> PermissionGroups { get ; set ; }

		public PermissionGroup FindPermissionGroup ( Guid guid )
		{
			return PermissionGroups . SingleOrDefault ( permissionGroup => permissionGroup . Guid == guid ) ;
		}

		public Entity FindEntity ( Guid guid )
		{
			return Entities . SingleOrDefault ( entity => entity . Guid == guid ) ;
		}

		public KnownSpecialGroups KnownSpecialGroups { get ; set ; }

		public void CreateNew ( ) { }

		public void Init ( )
		{
			InitializeEntities ( ) ;
			InitializeGroupMembers ( ) ;
			InitializePermissionGroups ( ) ;
			InitializeProperties ( ) ;
		}


		private void InitializeGroupMembers ( )
		{
			DbSet <DbGroupMember> dbGroupMembers = DatabaseStorage . DbGroupMembers ;

			foreach ( DbGroupMember dbGroupMember in dbGroupMembers )
			{
				Group group = FindEntity ( dbGroupMember . Group . Guid ) as Group ;

				if ( group == null )
				{
					//todo: Warning
					continue ;
				}

				Entity target = FindEntity ( dbGroupMember . MemberGuid ) ;

				if ( target == null )
				{
					//todo: Warning
					continue ;
				}

				group . Members . Add ( target ) ;
			}
		}

		private void InitializePermissionGroups ( )
		{
			DbSet <DbPermissionGroup> dbPermissionGroups = DatabaseStorage . DbPermissionGroups ;

			foreach ( DbPermissionGroup dbPermissionGroup in dbPermissionGroups )
			{
				Directory . Logic . PermissionGroup clientPermissionGroup =
					JsonSerializer . Deserialize <Directory . Logic . PermissionGroup> ( dbPermissionGroup . Value ) ;

				if ( clientPermissionGroup is null )
				{
					//todo: Warning
					continue ;
				}

				if ( clientPermissionGroup . Guid != dbPermissionGroup . Guid )
				{
					//todo: Warning
					continue ;
				}

				PermissionGroup permissionGroup = new PermissionGroup ( ) ;
				permissionGroup . Edit ( clientPermissionGroup ) ;
				permissionGroup . Guid = clientPermissionGroup . Guid ;

				PermissionGroups . Add ( permissionGroup ) ;
			}
		}

		private void InitializeProperties ( )
		{
			DbSet <DbProperty> dbProperties = DatabaseStorage . DbProperties ;

			foreach ( DbProperty dbProperty in dbProperties )
			{
				Entity propertyTarget = FindEntity ( dbProperty . Target ) ;

				if ( propertyTarget is null )
				{
					//todo: Warning
					continue ;
				}

				Entity propertyOwner = FindEntity ( dbProperty . Owner ) ?? KnownSpecialGroups . DirectoryServices ;

				PermissionGroup permissionGroup = FindPermissionGroup ( dbProperty . PermissionGuid )
												?? KnownPermissionGroups . InternalApiOnly ;

				EntityProperty property = new EntityProperty
										{
											Name        = dbProperty . Name ,
											Owner       = propertyOwner ,
											Permissions = permissionGroup ,
											Value       = dbProperty . Value ,
										} ;

				propertyTarget . Properties . Add ( property ) ;
			}
		}

		private void InitializeEntities ( )
		{
			KnownSpecialGroups = new KnownSpecialGroups ( ) ;

			DirectoryServices ??= new HashSet <DirectoryService> ( ) ;
			DbSet <DbDirectoryService> dbDirectoryServices = DatabaseStorage . DbDirectoryServices ;
			foreach ( DbDirectoryService dbDirectoryService in dbDirectoryServices )
			{
				DirectoryService directoryService =
					DirectoryServices . FirstOrDefault ( service => service . Guid == dbDirectoryService . Guid ) ;
				if ( directoryService is null )
				{
					directoryService = new DirectoryService { Guid = dbDirectoryService . Guid , } ;
					DirectoryServices . Add ( directoryService ) ;
				}
			}

			LoginServices ??= new HashSet <LoginService> ( ) ;
			DbSet <DbLoginService> dbLoginServices = DatabaseStorage . DbLoginServices ;
			foreach ( DbLoginService dbLoginService in dbLoginServices )
			{
				LoginService loginService =
					LoginServices . FirstOrDefault ( service => service . Guid == dbLoginService . Guid ) ;
				if ( loginService is null )
				{
					loginService = new LoginService { Guid = dbLoginService . Guid , } ;
					LoginServices . Add ( loginService ) ;
				}
			}

			Services ??= new HashSet <Service> ( ) ;
			DbSet <DbService> dbServices = DatabaseStorage . DbServices ;
			foreach ( DbService dbService in dbServices )
			{
				Service service = Services . FirstOrDefault ( service => service . Guid == dbService . Guid ) ;
				if ( service is null )
				{
					service = new Service { Guid = dbService . Guid , } ;
					Services . Add ( service ) ;
				}
			}

			Groups ??= new HashSet <Group> ( ) ;
			DbSet <DbGroup> dbGroups = DatabaseStorage . DbGroups ;
			foreach ( DbGroup dbGroup in dbGroups )
			{
				Group group = Groups . FirstOrDefault ( group => group . Guid == dbGroup . Guid ) ;
				if ( group is null )
				{
					group = new Group { Guid = dbGroup . Guid , } ;
					Groups . Add ( group ) ;
				}
			}

			Users ??= new HashSet <User> ( ) ;
			DbSet <DbUser> dbUsers = DatabaseStorage . DbUsers ;
			foreach ( DbUser dbUser in dbUsers )
			{
				User user = Users . FirstOrDefault ( user => user . Guid == dbUser . Guid ) ;
				if ( user is null )
				{
					user = new User { Guid = dbUser . Guid , } ;
					Users . Add ( user ) ;
				}
			}

			//todo
		}

	}

}
