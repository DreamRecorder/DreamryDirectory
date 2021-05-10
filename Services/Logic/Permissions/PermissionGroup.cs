﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . General ;
using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic . Permissions
{

	public class PermissionGroup
	{

		public Guid Guid { get ; set ; }

		public Entity Owner { get ; set ; }

		public HashSet <Permission> Permissions { get ; set ; } = new HashSet <Permission> ( ) ;

		public Directory . Logic . PermissionGroup ToClientSidePermissionGroup ( )
		{
			Directory . Logic . PermissionGroup result = new Directory . Logic . PermissionGroup ( ) ;

			result . Guid  = Guid ;
			result . Owner = Owner . Guid ;
			result . Permissions = Permissions . Select ( permission => permission . ToClientSidePermission ( ) ) .
												ToHashSet ( ) ;

			return result ;
		}

		public void Edit ( [NotNull] Directory . Logic . PermissionGroup permissionGroup )
		{
			if ( permissionGroup == null )
			{
				throw new ArgumentNullException ( nameof ( permissionGroup ) ) ;
			}

			Entity newOwner =
				DirectoryServiceInternal . Current . DirectoryDatabase . FindEntity ( permissionGroup . Owner ) ;

			if ( newOwner == null )
			{
				throw new TargetEntityNotFoundException ( permissionGroup . Owner ) ;
			}

			HashSet <Permission> newPermissions = new HashSet <Permission> ( ) ;

			foreach ( Directory . Logic . Permission permission in permissionGroup . Permissions )
			{
				newPermissions . Add ( Permission . Create ( permission ) ) ;
			}

			Permissions = newPermissions ;
		}

		public AccessType Access ( Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			bool ? write = null ;
			bool ? read  = null ;

			List <Permission> affectedPermission =
				Permissions . Where ( perm => perm . Target . Contain ( entity ) ) . ToList ( ) ;

			foreach ( Permission permission in affectedPermission )
			{
				switch ( permission . Status )
				{
					case PermissionStatus . Allow :
					{
						switch ( permission . Type )
						{
							case PermissionType . Read :
							{
								if ( read != false )
								{
									read = true ;
								}

								break ;
							}
							case PermissionType . Write :
							{
								if ( write != false )
								{
									write = true ;
								}

								break ;
							}
						}

						break ;
					}
					case PermissionStatus . Deny :
					{
						switch ( permission . Type )
						{
							case PermissionType . Read :
							{
								read = false ;
								break ;
							}
							case PermissionType . Write :
							{
								write = false ;
								break ;
							}
						}

						break ;
					}
				}
			}

			bool writeResult = write ?? false ;
			bool readResult  = read  ?? false ;

			if ( readResult )
			{
				if ( writeResult )
				{
					return AccessType . ReadWrite ;
				}
				else
				{
					return AccessType . Read ;
				}
			}
			else
			{
				return AccessType . None ;
			}
		}

	}

}
