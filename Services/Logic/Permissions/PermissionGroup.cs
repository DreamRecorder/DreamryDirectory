using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic . Permissions
{

	public class PermissionGroup
	{

		public HashSet <Permission> Permissions { get ; set ; } = new HashSet <Permission> ( ) ;

		public override string ToString ( )
		{
			StringBuilder stringBuilder = new StringBuilder ( ) ;

			foreach ( Permission permission in Permissions )
			{
				stringBuilder . AppendLine ( permission . ToString ( ) ) ;
			}

			return stringBuilder . ToString ( ) ;
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
