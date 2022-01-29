using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic . Tokens
{

	public class AccessToken : Token , IEquatable <AccessToken>
	{

		public Guid Target { get ; set ; }

		public bool Equals ( AccessToken other )
		{
			if ( ReferenceEquals ( null , other ) )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return base . Equals ( other ) && Target . Equals ( other . Target ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
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

			return Equals ( ( AccessToken )obj ) ;
		}

		public override int GetHashCode ( )
			=> HashCode . Combine ( base . GetHashCode ( ) , Target ) ;

		public static bool operator == ( AccessToken left , AccessToken right )
			=> Equals ( left , right ) ;

		public static bool operator != ( AccessToken left , AccessToken right )
			=> ! Equals ( left , right ) ;

	}

}
