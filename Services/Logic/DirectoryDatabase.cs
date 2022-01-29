using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . Specialized ;
using System . ComponentModel ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Storage ;
using DreamRecorder . ToolBox . General ;

using JetBrains . Annotations ;

using LazyCache ;

using PermissionGroup =
	DreamRecorder . Directory . Services . Logic . Permissions . PermissionGroup ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class DirectoryDatabase : IDirectoryDatabase
	{

		public IDirectoryDatabaseStorage DatabaseStorage { get ; set ; }

		public IDirectoryServiceInternal DirectoryServiceInternal { get ; set ; }

		public bool Initiated { get ; private set ; } = false ;

		protected IAppCache Cache { get ; set ; }

		public Anonymous Anonymous { get ; set ; } = new Anonymous ( ) ;

		public DirectoryDatabase (
			IDirectoryDatabaseStorage databaseStorage ,
			IAppCache                 cache ,
			IDirectoryServiceInternal directoryServiceInternal )
		{
			DatabaseStorage          = databaseStorage ;
			Cache                    = cache ;
			DirectoryServiceInternal = directoryServiceInternal ;
		}

		public PermissionGroup FindPermissionGroup ( Guid guid )
		{
			return Cache . GetOrAdd (
									CachePermissionGroupKey ( guid ) ,
									( ) => GetPermissionGroup ( guid ) ) ;
		}


		public Entity FindEntity ( Guid guid )
		{
			return Cache . GetOrAdd ( CacheEntityKey ( guid ) , ( ) => GetEntity ( guid ) ) ;
		}

		public EntityProperty FindProperty ( Guid entity , string name )
		{
			return Cache . GetOrAdd (
									CacheEntityPropertyKey ( entity , name ) ,
									( ) => GetProperty ( entity , name ) ) ;
		}

		public Entity AddEntity ( [NotNull] Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( DatabaseStorage . DbEntities . Find ( entity . Guid ) is DbEntity dbEntity )
			{
				//Todo:
			}
			else
			{
				dbEntity = new DbEntity ( ) ;
				DatabaseStorage . DbEntities . Add ( dbEntity ) ;
			}

			dbEntity . Guid = entity . Guid ;

			switch ( entity )
			{
				case Anonymous anonymous :
				{
					dbEntity . EntityType = DbEntityType . Anonymous ;
					break ;
				}
				case DirectoryService directoryService :
					dbEntity . EntityType = DbEntityType . DirectoryService ;
					break ;
				case Group group :
					dbEntity . EntityType = DbEntityType . Group ;

					foreach ( Guid member in group . Members )
					{
						if ( DatabaseStorage . DbGroupMembers . Find ( group . Guid , member )
						== null )
						{
							DatabaseStorage . DbGroupMembers . Add (
							new DbGroupMember
							{
								GroupGuid = group . Guid , MemberGuid = member ,
							} ) ;
						}
					}

					group . Members . CollectionChanged += GetMembersCollectionChanged ( group ) ;

					break ;
				case LoginService loginService :
					dbEntity . EntityType = DbEntityType . LoginService ;
					break ;
				case Service service :
					dbEntity . EntityType = DbEntityType . Service ;
					break ;
				case SpecialGroup specialGroup :
					dbEntity . EntityType = DbEntityType . SpecialGroup ;
					break ;
				case User user :
					dbEntity . EntityType = DbEntityType . User ;
					break ;

				default :
					throw new ArgumentOutOfRangeException ( nameof ( entity ) ) ;
			}

			Cache . Add ( CacheEntityKey ( entity . Guid ) , entity ) ;

			DatabaseStorage . Save ( ) ;

			return entity ;
		}

		public PermissionGroup AddPermissionGroup ( [NotNull] PermissionGroup permissionGroup )
		{
			if ( permissionGroup == null )
			{
				throw new ArgumentNullException ( nameof ( permissionGroup ) ) ;
			}

			if ( DatabaseStorage . DbPermissionGroups . Find ( permissionGroup . Guid ) is
				DbPermissionGroup dbPermissionGroup )
			{
			}
			else
			{
				dbPermissionGroup = new DbPermissionGroup ( ) ;
				DatabaseStorage . DbPermissionGroups . Add ( dbPermissionGroup ) ;
			}

			dbPermissionGroup . Guid  = permissionGroup . Guid ;
			dbPermissionGroup . Owner = permissionGroup . Owner ;
			dbPermissionGroup . Value = permissionGroup . Permissions . CastToBytes ( ) ;

			Cache . Add ( CachePermissionGroupKey ( permissionGroup . Guid ) , permissionGroup ) ;

			DatabaseStorage . Save ( ) ;

			return permissionGroup ;
		}

		public void AddEntityProperty ( [NotNull] EntityProperty property )
		{
			if ( property == null )
			{
				throw new ArgumentNullException ( nameof ( property ) ) ;
			}

			Cache . Add (
						CacheEntityPropertyKey ( property . Target , property . Name ) ,
						property ) ;
			SaveProperty ( property ) ;
		}

		public KnownSpecialGroups KnownSpecialGroups { get ; set ; } = new KnownSpecialGroups ( ) ;

		public void Save ( ) { }

		public void Initiate ( ) { }

		public void CreateNew ( ) { }

		private PermissionGroup GetPermissionGroup ( Guid guid )
		{
			if ( DatabaseStorage . DbPermissionGroups . Find ( guid ) is DbPermissionGroup
				dbPermissionGroup )
			{
				PermissionGroup permissionGroup =
					new PermissionGroup { Guid = guid , Owner = dbPermissionGroup . Owner , } ;

				permissionGroup . Permissions . UnionWith (
															dbPermissionGroup . Value .
																CastToStructs <Permission> ( ) ) ;

				permissionGroup . Permissions . CollectionChanged +=
					GetPermissionsCollectionChanged ( permissionGroup ) ;

				return permissionGroup ;
			}
			else
			{
				return null ;
			}
		}

		private NotifyCollectionChangedEventHandler GetPermissionsCollectionChanged (
			PermissionGroup permissionGroup )
		{
			void PermissionsCollectionChanged ( object sender , NotifyCollectionChangedEventArgs e )
			{
				SavePermissionGroup ( permissionGroup ) ;
			}

			return PermissionsCollectionChanged ;
		}

		private void Property_PropertyChanged ( object sender , PropertyChangedEventArgs e )
		{
			if ( sender is EntityProperty property )
			{
				SaveProperty ( property ) ;
			}
		}

		private NotifyCollectionChangedEventHandler GetMembersCollectionChanged ( Group group )
		{
			void MembersCollectionChanged ( object sender , NotifyCollectionChangedEventArgs e )
			{
				if ( e ? . OldItems != null )
				{
					foreach ( Guid guid in e ? . OldItems )
					{
						if ( DatabaseStorage . DbGroupMembers . Find ( group . Guid , guid ) is
							DbGroupMember dbGroupMember )
						{
							DatabaseStorage . DbGroupMembers . Remove ( dbGroupMember ) ;
						}
					}
				}

				if ( e ? . NewItems != null )
				{
					foreach ( Guid guid in e ? . NewItems )
					{
						if ( DatabaseStorage . DbGroupMembers . Find ( group . Guid , guid )
						== null )
						{
							DatabaseStorage . DbGroupMembers . Add (
							new DbGroupMember
							{
								GroupGuid = group . Guid , MemberGuid = guid ,
							} ) ;
						}
					}
				}
			}

			return MembersCollectionChanged ;
		}

		private Entity GetEntity ( Guid guid )
		{
			Entity entity = null ;

			if ( DatabaseStorage . DbEntities . Find ( guid ) is DbEntity dbEntity )
			{
				switch ( dbEntity . EntityType )
				{
					case DbEntityType . DirectoryService :
					{
						entity = new DirectoryService ( ) ;
						break ;
					}
					case DbEntityType . LoginService :
					{
						entity = new LoginService ( ) ;
						break ;
					}
					case DbEntityType . Group :
					{
						Group group = new Group ( ) ;

						group . Members . UnionWith (
													DatabaseStorage . DbGroupMembers .
														Where (
																member
																	=> member . GroupGuid
																	== guid ) .
														Select ( member => member . MemberGuid ) ) ;


						group . Members . CollectionChanged +=
							GetMembersCollectionChanged ( group ) ;

						entity = group ;

						break ;
					}
					case DbEntityType . Service :
					{
						entity = new Service ( ) ;
						break ;
					}
					case DbEntityType . User :
					{
						entity = new User ( ) ;
						break ;
					}
					case DbEntityType . SpecialGroup :
					{
						entity = KnownSpecialGroups . Entities . FirstOrDefault (
						specialGroup => specialGroup . Guid == guid ) ;
						break ;
					}
					case DbEntityType . Anonymous :
					{
						entity = Anonymous ;
						break ;
					}
					default :
					{
						entity = null ;
						throw new InvalidOperationException ( ) ;
					}
				}

				entity . Guid = dbEntity . Guid ;
			}

			return entity ;
		}

		private EntityProperty GetProperty ( Guid entity , string name )
		{
			if ( DatabaseStorage . DbProperties . Find ( entity , name ) is DbProperty dbProperty )
			{
				EntityProperty property = new EntityProperty
										{
											Name   = dbProperty . Name ,
											Owner  = dbProperty . Owner ,
											Target = dbProperty . Target ,
											Permissions =
												FindPermissionGroup ( dbProperty . Permission ) ,
											Value = dbProperty . Value ,
										} ;

				property . PropertyChanged += Property_PropertyChanged ;

				return property ;
			}
			else
			{
				return null ;
			}
		}

		public string CacheEntityPropertyKey ( Guid entity , string name )
			=> $"({nameof ( EntityProperty )}){entity}.{name}" ;

		public string CachePermissionGroupKey ( Guid guid )
			=> $"({nameof ( PermissionGroup )}){guid}" ;

		public string CacheEntityKey ( Guid guid ) => $"({nameof ( Entity )}){guid}" ;

		private void SavePermissionGroup ( PermissionGroup permissionGroup ) { }

		private void SaveProperty ( EntityProperty property )
		{
			DbProperty dbProperty =
				DatabaseStorage . DbProperties . Find ( property . Target , property . Name ) ;

			if ( dbProperty != null )
			{
				dbProperty . Value      = property . Value ;
				dbProperty . Owner      = property . Owner ;
				dbProperty . Permission = property . Permissions . Guid ;
			}
			else
			{
				dbProperty = new DbProperty
							{
								Value      = property . Value ,
								Owner      = property . Owner ,
								Permission = property . Permissions . Guid ,
								Target     = property . Target ,
								Name       = property . Name ,
							} ;

				DatabaseStorage . DbProperties . Add ( dbProperty ) ;
			}

			lock ( DatabaseStorage )
			{
				DatabaseStorage . Save ( ) ;
			}
		}

	}

}
