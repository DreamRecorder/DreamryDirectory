using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . General
{

	public class TokenStorage <TToken> where TToken : Token
	{

		public HashSet <TToken> Tokens { get ; set ; }

		public void Gc ( ) { Tokens . RemoveWhere ( token => token . NotAfter < DateTimeOffset . UtcNow ) ; }

		public void AddToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( token . NotAfter > DateTimeOffset . UtcNow )
			{
				Tokens . Add ( token ) ;
			}
		}

		public void DisposeToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			Tokens . Remove ( token ) ;
		}

		public void CheckToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( Tokens . Contains ( token ) )
			{
			}
			else
			{
				throw new InvalidTokenException ( ) ;
			}
		}

	}

}
