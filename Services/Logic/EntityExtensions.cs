using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class GroupExtensions
	{

		public static readonly string Members = nameof ( Members ) ;

		public static readonly string MembersName = $"{Consts . Namespace}.{Members}" ;

		public static EntityProperty GetMembersProperty ( [NotNull] Group group )
		{
			if ( @group == null )
			{
				throw new ArgumentNullException ( nameof ( @group ) ) ;
			}

			if ( group . Properties . FirstOrDefault ( ( prop ) => prop . Name == Members ) is EntityProperty property )
			{
				return property ;
			}
			else
			{
				property = new EntityProperty ( )
							{
								Name  = MembersName ,
								Guid  = Guid . NewGuid ( ) ,
								Owner = DirectoryServiceInternal . Current . KnownSpecialGroups . DirectoryServices ,
								Value = string . Empty
							} ;


			}
		}

	}

	public static class EntityExtensions
	{

		public static readonly string IsDisabled = nameof ( IsDisabled ) ;

		public static readonly string IsDisabledName = $"{Consts . Namespace}.{IsDisabled}" ;

		public static bool GetIsDisabled ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( entity . Properties . FirstOrDefault ( prop => prop . Name == IsDisabledName ) is EntityProperty
					attribute )
			{
				if ( bool . TryParse ( attribute . Value , out bool result ) )
				{
					return result ;
				}
			}

			return false ;
		}

		public static char CanLoginFromSeparator => ',' ;

		public static readonly string CanLoginFrom = nameof ( CanLoginFrom ) ;

		public static readonly string CanLoginFromName = $"{Consts . Namespace}.{CanLoginFrom}" ;

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
								Name        = CanLoginFromName ,
								Owner       = DirectoryServiceInternal . Current . ServiceEntity ,
								Guid        = Guid . NewGuid ( ) ,
								Permissions = new PermissionGroup ( ) ,
							} ;

				property . Permissions . Permissions . Add (
															new Permission ( )
															{
																Target = DirectoryServiceInternal . Current .
																	KnownSpecialGroups . DirectoryServices ,
																Type   = PermissionType . Write ,
																Status = PermissionStatus . Allow
															} ) ;

				property . Permissions . Permissions . Add (
															new Permission ( )
															{
																Target = DirectoryServiceInternal . Current .
																	KnownSpecialGroups . DirectoryServices ,
																Type   = PermissionType . Read ,
																Status = PermissionStatus . Allow
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
