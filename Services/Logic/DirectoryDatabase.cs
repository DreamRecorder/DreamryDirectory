﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;
using DreamRecorder . Directory . Services . Logic . Storage ;

using JetBrains . Annotations ;

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
						Union ( KnownSpecialGroups . Entities ) ;

		public DirectoryDatabase ( IDirectoryDatabaseStorage databaseStorage ) { DatabaseStorage = databaseStorage ; }

		public HashSet <User> Users { get ; set ; }

		public HashSet <Group> Groups { get ; set ; }

		public HashSet <Service> Services { get ; set ; }

		public HashSet <LoginService> LoginServices { get ; set ; }

		public HashSet <DirectoryService> DirectoryServices { get ; set ; }

		public Entity FindEntity ( Guid guid )
		{
			return Entities . SingleOrDefault ( entity => entity . Guid == guid ) ;
		}

		public KnownSpecialGroups KnownSpecialGroups { get ; set ; }

		public void Init ( ) { InitializeEntities ( ) ; }

		public PermissionGroup CreatePermissionGroup ( [NotNull] string value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException ( nameof ( value ) ) ;
			}

			PermissionGroup result = new PermissionGroup ( ) ;

			List <string [ ]> permissions = value .
											Split ( Environment . NewLine , StringSplitOptions . RemoveEmptyEntries ) .
											Select ( line => line . Trim ( ) ) .
											Select (
													line
														=> line .
															Split ( ',' , StringSplitOptions . RemoveEmptyEntries ) .
															Select ( part => part . Trim ( ) ) .
															ToArray ( ) ) .
											ToList ( ) ;

			foreach ( string [ ] permissionStrings in permissions )
			{
				if ( permissionStrings . Length == 3 )
				{
					Guid   guid   = Guid . Parse ( permissionStrings [ 0 ] ) ;
					Entity target = FindEntity ( guid ) ;

					if ( target != null )
					{
						Permission permission = new Permission (
																target ,
																Enum . Parse <PermissionStatus> (
																permissionStrings [ 1 ] ) ,
																Enum . Parse <PermissionType> (
																permissionStrings [ 2 ] ) ) ;

						result . Permissions . Add ( permission ) ;
					}
				}
				else
				{
				}
			}

			return result ;
		}


		private void InitProp ( )
		{
			foreach ( DirectoryService directoryService in DirectoryServices )
			{
				foreach ( DbProperty dbProperty in directoryService . DatabaseObject . Proprieties )
				{
					Entity propertyOwner = FindEntity ( dbProperty . Owner ) ;

					if ( propertyOwner is null )
					{
						propertyOwner = DirectoryServiceInternal . ServiceEntity ;
					}

					EntityProperty property = new EntityProperty
											{
												Name        = dbProperty . Name ,
												Owner       = propertyOwner ,
												Permissions = CreatePermissionGroup ( dbProperty . Permission ) ,
												Value       = dbProperty . Value ,
											} ;

					directoryService . Properties . Add ( property ) ;
				}
			}
		}

		private void InitializeEntities ( )
		{
			KnownSpecialGroups = new KnownSpecialGroups ( ) ;

			DirectoryServices ??= new HashSet <DirectoryService> ( ) ;
			HashSet <DbDirectoryService> dbDirectoryServices = DatabaseStorage . GetDbDirectoryServices ( ) ;
			foreach ( DbDirectoryService dbDirectoryService in dbDirectoryServices )
			{
				DirectoryService directoryService =
					DirectoryServices . FirstOrDefault ( service => service . Guid == dbDirectoryService . Guid ) ;
				if ( directoryService is null )
				{
					directoryService = new DirectoryService
										{
											Guid = dbDirectoryService . Guid , DatabaseObject = dbDirectoryService
										} ;
					DirectoryServices . Add ( directoryService ) ;
				}
				else
				{
					directoryService . DatabaseObject = dbDirectoryService ;
				}
			}

			//todo
		}

	}

}
