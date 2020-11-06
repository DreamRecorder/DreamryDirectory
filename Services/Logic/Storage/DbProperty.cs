using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbProperty : IEquatable <DbProperty>
	{

		public bool Equals ( DbProperty other )
		{
			if ( other is null)
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Owner . Equals ( other . Owner ) && Name == other . Name ;
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

			return Equals ( ( DbProperty ) obj ) ;
		}

		public override int GetHashCode ( ) { return HashCode . Combine ( Owner , Name ) ; }

		public static bool operator == ( DbProperty left , DbProperty right ) { return Equals ( left , right ) ; }

		public static bool operator != ( DbProperty left , DbProperty right ) { return ! Equals ( left , right ) ; }

		public Guid Owner { get ; set ; }

		public string Name { get ; set ; }

		public string Value { get ; set ; }

		public string Permission { get ; set ; }

	}

}