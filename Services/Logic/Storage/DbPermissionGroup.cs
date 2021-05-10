using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbPermissionGroup : IEquatable <DbPermissionGroup>
	{

		public Guid Guid { get ; set ; }

		public string Value { get ; set ; }

		public bool Equals ( DbPermissionGroup other )
		{
			if ( other is null )
			{
				return false ;
			}

			return ReferenceEquals ( this , other ) || Guid . Equals ( other . Guid ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , obj ) )
			{
				return true ;
			}

			return obj . GetType ( ) == GetType ( ) && Equals ( ( DbPermissionGroup ) obj ) ;
		}

		public override int GetHashCode ( ) => Guid . GetHashCode ( ) ;

		public static bool operator == ( DbPermissionGroup left , DbPermissionGroup right ) => Equals ( left , right ) ;

		public static bool operator != ( DbPermissionGroup left , DbPermissionGroup right )
			=> ! Equals ( left , right ) ;

	}

}
