using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;

using JetBrains . Annotations ;

using static DreamRecorder . Directory . Logic . KnownProperties ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class EntityExtensions
	{

		public static char CanLoginFromSeparator => ',' ;


		public static string DisplayNameName => DisplayName . ToPropertyName ( ) ;


		public static string IsDisabledName => IsDisabled . ToPropertyName ( ) ;

		public static string CanLoginFrom => nameof ( CanLoginFrom ) ;

		public static string CanLoginFromName => CanLoginFrom . ToPropertyName ( ) ;

		public static string StopRenewEntityToken => nameof ( StopRenewEntityToken ) ;

		public static string StopRenewEntityTokenName
			=> $"{Constants . Namespace}.{StopRenewEntityToken}" ;

		[CanBeNull]
		public static EntityProperty TryGetProperty (
			[NotNull] this Entity entity ,
			[NotNull]      string name )
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindProperty (
			entity . Guid ,
			name ) ;

		public static EntityProperty GetOrCreateProperty (
			[NotNull] this Entity entity ,
			[NotNull]      string name ,
			Guid ?                owner           = default ,
			PermissionGroup       permissionGroup = default ,
			string                defaultValue    = default )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( name == null )
			{
				throw new ArgumentNullException ( nameof ( name ) ) ;
			}

			Guid ownerResult = owner
							?? DirectoryServiceInternal . KnownSpecialGroups . DirectoryServices .
														Guid ;

			permissionGroup ??= KnownPermissionGroups . InternalApiOnly ;

			if ( permissionGroup == null )
			{
				throw new ArgumentNullException ( nameof ( permissionGroup ) ) ;
			}

			if ( entity . TryGetProperty ( name ) is not EntityProperty property )
			{
				property = new EntityProperty
							{
								Name        = name ,
								Owner       = ownerResult ,
								Permissions = permissionGroup ,
								Value       = defaultValue ,
								Target      = entity . Guid ,
							} ;

				DirectoryServiceInternal . Current . DirectoryDatabase .
											AddEntityProperty ( property ) ;
			}

			return property ;
		}

		public static EntityProperty GetDisplayNameProperty ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			switch ( entity )
			{
				case Group group :
				{
					return entity . GetOrCreateProperty (
														DisplayNameName ,
														group . GetMembersProperty ( ) . Owner ,
														KnownPermissionGroups .
															AuthorizedReadonly ) ;
				}
				case User user :
				case Service service :
				{
					return entity . GetOrCreateProperty (
														DisplayNameName ,
														entity . Guid ,
														KnownPermissionGroups .
															AuthorizedReadonly ) ;
				}
				case LoginService :
				case DirectoryService :
				{
					return entity . GetOrCreateProperty (
														DisplayNameName ,
														entity . Guid ,
														KnownPermissionGroups . EveryoneReadonly ) ;
				}
				default :
				{
					return entity . GetOrCreateProperty (
														DisplayNameName ,
														permissionGroup : KnownPermissionGroups .
															AuthorizedReadonly ) ;
				}
			}
		}

		[CanBeNull]
		public static string GetDisplayName ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			return entity . GetDisplayNameProperty ( ) . Value ;
		}

		public static EntityProperty GetIsDisabledProperty ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			return entity . GetOrCreateProperty (
												IsDisabledName ,
												permissionGroup : KnownPermissionGroups .
													AuthorizedReadonly ) ;
		}


		public static bool GetIsDisabled ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			return Convert . ToBoolean ( entity . GetIsDisabledProperty ( ) . Value ) ;
		}

		public static bool SetIsDisabled ( [NotNull] this Entity entity , bool isDisabled )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			entity . GetIsDisabledProperty ( ) . Value = isDisabled . ToString ( ) ;
			return isDisabled ;
		}

		public static EntityProperty GetCanLoginFromProperty ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			return entity . GetOrCreateProperty (
												CanLoginFromName ,
												permissionGroup : KnownPermissionGroups .
													LoginServicesReadonly ) ;
		}

		public static ICollection <Guid> GetCanLoginFrom ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			return entity . GetCanLoginFromProperty ( ) .
							Value . Split (
											CanLoginFromSeparator ,
											StringSplitOptions . RemoveEmptyEntries ) .
							Select ( Guid . Parse ) .
							ToHashSet ( ) ;
		}

		public static ICollection <Guid> AddCanLoginFrom (
			[NotNull] this Entity entity ,
			LoginService          loginService )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			ICollection <Guid> canLoginFrom = GetCanLoginFrom ( entity ) ;

			canLoginFrom . Add ( loginService . Guid ) ;

			entity . GetCanLoginFromProperty ( ) . Value = string . Concat (
			canLoginFrom . Select ( guid => guid . ToString ( ) + CanLoginFromSeparator ) ) ;

			return canLoginFrom ;
		}

	}

}
