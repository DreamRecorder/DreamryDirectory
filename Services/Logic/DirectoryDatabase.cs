using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System.Runtime.CompilerServices;
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

		public bool Initiated { get; private set; } = false;

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

		public void Save ( )
		{
			SavePermissionGroups ( ) ;
			SaveEntities ( ) ;
			SaveProperties ( ) ;
			SaveGroupMembers ( ) ;
		}

		private void SaveProperties ( )
		{
			

		}

		private void SavePermissionGroups ( )
		{
			PermissionGroups ??= new HashSet <PermissionGroup> ( ) ;
			DbSet <DbPermissionGroup> dbPermissionGroups = DatabaseStorage . DbPermissionGroups ;
			foreach ( PermissionGroup permissionGroup in PermissionGroups )
			{
				DbPermissionGroup dbPermissionGroup = dbPermissionGroups . Find ( permissionGroup . Guid ) ;

				if ( dbPermissionGroup is null )
				{
					dbPermissionGroup = new DbPermissionGroup ( ) { Guid = permissionGroup . Guid , } ;
					dbPermissionGroups . Add ( dbPermissionGroup ) ;
				}

				dbPermissionGroup . Value =
					JsonSerializer . Serialize ( permissionGroup . ToClientSidePermissionGroup ( ) ) ;
			}

			DatabaseStorage . Save ( ) ;
		}

		private void SaveGroupMembers ( )
		{
			DbSet <DbGroupMember> dbGroupMembers = DatabaseStorage . DbGroupMembers ;

			dbGroupMembers . RemoveRange (
										dbGroupMembers . SkipWhile (
																	dbGroupMember
																		=> Groups . Any (
																		group
																			=> group . Guid
																				== dbGroupMember . GroupGuid ) ) ) ;

			foreach ( Group @group in Groups )
			{
				dbGroupMembers . RemoveRange (
											dbGroupMembers .
												Where ( dbGroupMember => dbGroupMember . GroupGuid == group . Guid ) .
												SkipWhile (
															dbGroupMember
																=> group . Members . Any (
																member
																	=> member . Guid
																		== dbGroupMember . MemberGuid ) ) ) ;

				foreach ( Entity groupMember in group . Members )
				{
					DbGroupMember dbGroupMember = dbGroupMembers . Find ( group . Guid , groupMember . Guid ) ;

					if ( dbGroupMember is null )
					{
						dbGroupMember =
							new DbGroupMember ( ) { GroupGuid = group . Guid , MemberGuid = groupMember . Guid } ;

						dbGroupMembers . Add ( dbGroupMember ) ;
					}
				}
			}
		}

		public void SaveEntitiesType<T> (DbSet<T> storageSet) where T: class , IDbEntity
		{
			DbSet <DbDirectoryService> dbDirectoryServices = DatabaseStorage . DbDirectoryServices ;

			dbDirectoryServices . RemoveRange (
												dbDirectoryServices . SkipWhile (
												dbDirectoryService
													=> DirectoryServices . Any (
																				( directoryService
																						=> directoryService . Guid
																							== dbDirectoryService .
																								Guid ) ) ) ) ;

			foreach ( DirectoryService directoryService in DirectoryServices )
			{
				DbDirectoryService dbDirectoryService = dbDirectoryServices . Find ( directoryService . Guid ) ;

				if ( dbDirectoryService is null )
				{
					dbDirectoryService = new DbDirectoryService ( )
										{
											Guid = directoryService . Guid , Properties = new HashSet <DbProperty> ( ) ,
										} ;

					dbDirectoryServices . Add ( dbDirectoryService ) ;
				}
			}

		}

		public void SaveEntities ( )
		{
			DirectoryServices ??= new HashSet <DirectoryService> ( ) ;

			DbSet <DbDirectoryService> dbDirectoryServices = DatabaseStorage . DbDirectoryServices ;

			dbDirectoryServices . RemoveRange (
												dbDirectoryServices . SkipWhile (
												dbDirectoryService
													=> DirectoryServices . Any (
																				( directoryService
																						=> directoryService . Guid
																							== dbDirectoryService .
																								Guid ) ) ) ) ;

			foreach ( DirectoryService directoryService in DirectoryServices )
			{
				DbDirectoryService dbDirectoryService = dbDirectoryServices . Find ( directoryService . Guid ) ;

				if ( dbDirectoryService is null )
				{
					dbDirectoryService = new DbDirectoryService ( )
										{
											Guid = directoryService . Guid , Properties = new HashSet <DbProperty> ( ) ,
										} ;

					dbDirectoryServices . Add ( dbDirectoryService ) ;
				}
			}

		DatabaseStorage . Save ( ) ;
		}

		public void CreateNew ( )
		{

		}

		public void Initiate ( )
		{
			if ( Initiated )
			{
				return ;
			}

			InitializeEntities ( ) ;
			InitializeGroupMembers ( ) ;
			InitializePermissionGroups ( ) ;
			InitializeProperties ( ) ;

			Initiated = true;
		}


		private void InitializeGroupMembers ( )
		{
			DbSet <DbGroupMember> dbGroupMembers = DatabaseStorage . DbGroupMembers ;

			foreach ( DbGroupMember dbGroupMember in dbGroupMembers )
			{
				Group group = FindEntity ( dbGroupMember . GroupGuid ) as Group ;

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

				if ( PermissionGroups . SingleOrDefault ( pg => pg . Guid == clientPermissionGroup . Guid ) is not
						PermissionGroup permissionGroup )
				{
					permissionGroup = new PermissionGroup { Guid = clientPermissionGroup . Guid } ;
					PermissionGroups . Add ( permissionGroup ) ;
				}

				permissionGroup . Edit ( clientPermissionGroup ) ;

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

				if ( propertyTarget . Properties . SingleOrDefault ( entityProperty => entityProperty . Name == dbProperty . Name ) is not
						EntityProperty property )
				{
					property = new EntityProperty { Name = dbProperty . Name , } ;
					propertyTarget . Properties . Add ( property ) ;
				}

				Entity propertyOwner = FindEntity ( dbProperty . Owner ) ?? KnownSpecialGroups . DirectoryServices ;

				PermissionGroup permissionGroup = FindPermissionGroup ( dbProperty . PermissionGuid )
												?? KnownPermissionGroups . InternalApiOnly ;

				property.Owner       = propertyOwner;
				property.Permissions = permissionGroup;
				property.Value       = dbProperty.Value;
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
