using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel . DataAnnotations ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbLoginService : IEquatable <DbLoginService>
	{

		[Required]
		public Guid Guid { get ; set ; }

		[Required]
		public HashSet <DbProperty> Properties { get ; set ; }

		public bool Equals ( DbLoginService other )
		{
			if ( other is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Guid . Equals ( other . Guid ) ;
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

			if ( obj . GetType ( ) != GetType ( ) )
			{
				return false ;
			}

			return Equals ( ( DbLoginService ) obj ) ;
		}

		public override int GetHashCode ( ) => Guid . GetHashCode ( ) ;

		public static bool operator == ( DbLoginService left , DbLoginService right ) => Equals ( left , right ) ;

		public static bool operator != ( DbLoginService left , DbLoginService right ) => ! Equals ( left , right ) ;

	}

}
