using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class EntityExtensions
	{
		public static readonly string DisplayName = nameof ( DisplayName ) ;

		public static readonly string DisplayNameName = $"{Constants . Namespace}.{DisplayName}" ;

		[CanBeNull]
		public static string GetDisplayName ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( entity . Properties . FirstOrDefault ( prop => prop . Name == DisplayNameName) is EntityProperty
					attribute )
			{
				return attribute . Value ;
			}

			return null ;
		}

		public static readonly string IsDisabled = nameof ( IsDisabled ) ;

		public static readonly string IsDisabledName = $"{Constants . Namespace}.{IsDisabled}" ;

		public static bool GetIsDisabled ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( entity . Properties . FirstOrDefault ( prop => prop . Name == IsDisabledName ) is EntityProperty
					property )
			{
				if ( bool . TryParse ( property . Value , out bool result ) )
				{
					return result ;
				}
			}

			return false ;
		}

		public static bool SetIsDisabled([NotNull] this Entity entity,bool isDisabled)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			EntityProperty property = entity.Properties.FirstOrDefault(prop => prop.Name == IsDisabledName) ;
			if (property is null )
			{
			
			}

			return false;
		}

		public static char CanLoginFromSeparator => ',' ;

		public static readonly string CanLoginFrom = nameof ( CanLoginFrom ) ;

		public static readonly string CanLoginFromName = $"{Constants . Namespace}.{CanLoginFrom}" ;

		public static ICollection <Guid> GetCanLoginFrom ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( entity . Properties . FirstOrDefault ( ( prop ) => prop . Name == CanLoginFromName ) is EntityProperty
					property )
			{
				return property . Value . Split ( CanLoginFromSeparator , StringSplitOptions . RemoveEmptyEntries ) .
								Select ( Guid . Parse ) .
								ToHashSet ( ) ;

				;
			}

			return new HashSet <Guid> ( ) ;
		}

		public static ICollection <Guid> AddCanLoginFrom ( [NotNull] this Entity entity , LoginService loginService )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			ICollection <Guid> canLoginFrom = GetCanLoginFrom ( entity ) ;

			canLoginFrom . Add ( loginService . Guid ) ;

			EntityProperty property =
				entity . Properties . FirstOrDefault ( ( prop ) => prop . Name == CanLoginFromName ) ;

			if ( property is null )
			{
				property = new EntityProperty ( )
							{
								Name = CanLoginFromName ,
								Owner = DirectoryServiceInternal . Current .DirectoryDatabase.KnownSpecialGroups . DirectoryServices ,
								Permissions = new PermissionGroup ( ) ,
							} ;

				property . Permissions . Permissions . Add (
															new Permission ( )
															{
																Target = DirectoryServiceInternal . Current .
																	DirectoryDatabase.KnownSpecialGroups . LoginServices ,
																Status = PermissionStatus . Allow ,
																Type   = PermissionType . Read
															} ) ;

				entity . Properties . Add ( property ) ;
			}

			property . Value = string . Concat (
												canLoginFrom . Select (
																		( guid )
																			=> guid . ToString ( )
																				+ CanLoginFromSeparator ) ) ;

			return canLoginFrom ;
		}

	}

}
