using System ;
using System . Collections ;
using System . Collections . Generic ;
using System.ComponentModel.DataAnnotations;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbGroupMember : IEquatable <DbGroupMember>
	{

		[Required]
		public Guid GroupGuid { get ; set ; }

		[Required]
		public Guid MemberGuid { get ; set ; }

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

			return GroupGuid . Equals ( other . GroupGuid ) && MemberGuid . Equals ( other . MemberGuid ) ;
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

		public override int GetHashCode ( ) => HashCode . Combine ( GroupGuid , MemberGuid ) ;

		public static bool operator == ( DbGroupMember left , DbGroupMember right ) => Equals ( left , right ) ;

		public static bool operator != ( DbGroupMember left , DbGroupMember right ) => ! Equals ( left , right ) ;

	}

}
