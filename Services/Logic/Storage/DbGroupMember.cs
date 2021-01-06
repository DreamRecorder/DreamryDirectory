using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbGroupMember : IEquatable <DbGroupMember>
	{

		public Guid Group { get ; set ; }

		public Guid Member { get ; set ; }

		public bool Equals ( DbGroupMember other )
		{
			if ( other is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Group . Equals ( other . Group ) && Member . Equals ( other . Member ) ;
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

			return Equals ( ( DbGroupMember ) obj ) ;
		}

		public override int GetHashCode ( ) { return HashCode . Combine ( Group , Member ) ; }

		public static bool operator == ( DbGroupMember left , DbGroupMember right ) { return Equals ( left , right ) ; }

		public static bool operator != ( DbGroupMember left , DbGroupMember right )
		{
			return ! Equals ( left , right ) ;
		}

	}

}
