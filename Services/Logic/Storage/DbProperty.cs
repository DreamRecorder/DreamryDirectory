using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel . DataAnnotations ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbProperty : IEquatable <DbProperty>
	{

		[Required]
		public Guid Target { get ; set ; }

		[Required]
		public Guid Owner { get ; set ; }

		[Required]
		public string Name { get ; set ; }

		[Required]
		public string Value { get ; set ; }

		[Required]
		public Guid PermissionGuid { get ; set ; }

		[Required]
		public DbPermissionGroup Permission { get ; set ; }

		public bool Equals ( DbProperty other )
		{
			if ( other is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Target . Equals ( other . Target ) && Name == other . Name ;
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

			return obj . GetType ( ) == GetType ( ) && Equals ( ( DbProperty ) obj ) ;
		}

		public override int GetHashCode ( ) => HashCode . Combine ( Target , Name ) ;

		public static bool operator == ( DbProperty left , DbProperty right ) => Equals ( left , right ) ;

		public static bool operator != ( DbProperty left , DbProperty right ) => ! Equals ( left , right ) ;

	}

}
