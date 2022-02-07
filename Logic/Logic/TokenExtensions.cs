using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Exceptions ;
using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Logic
{

	public static class TokenExtensions
	{

		/// <summary>
		///     Check if a token is in valid time
		/// </summary>
		/// <param name="token"></param>
		public static void CheckTokenTime ( [NotNull] this Token token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( token . NotAfter > DateTimeOffset . UtcNow
			&& token . NotBefore  < DateTimeOffset . UtcNow )
			{
			}
			else
			{
				throw new InvalidTimeException ( ) ;
			}
		}

	}

}
