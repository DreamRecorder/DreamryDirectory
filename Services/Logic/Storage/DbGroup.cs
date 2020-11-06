using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbGroup : IEquatable <DbGroup>
	{

		public bool Equals ( DbGroup other )
		{
			if ( other is null)
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
			if ( obj is null)
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

			return Equals ( ( DbGroup ) obj ) ;
		}

		public override int GetHashCode ( ) { return Guid . GetHashCode ( ) ; }

		public static bool operator == ( DbGroup left , DbGroup right ) { return Equals ( left , right ) ; }

		public static bool operator != ( DbGroup left , DbGroup right ) { return ! Equals ( left , right ) ; }

		public Guid Guid { get ; set ; }

		public HashSet <DbProperty> Proprieties { get ; set ; }

		public HashSet <DbGroupMember> Members { get ; set ; }

	}

}