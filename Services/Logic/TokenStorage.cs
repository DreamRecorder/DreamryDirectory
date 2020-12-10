using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic.Tokens;

using JetBrains . Annotations ;

namespace DreamRecorder.Directory.Services.Logic
{
	public static class TokenExtensions
	{

		/// <summary>
		/// Check if a token is in valid time
		/// </summary>
		/// <param name="token"></param>
		public static void CheckTokenTime([NotNull] this Token token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (token.NotAfter     > DateTimeOffset.UtcNow
				&& token.NotBefore < DateTimeOffset.UtcNow)
			{
				return;
			}
			else
			{
				throw new InvalidTimeException();
			}
		}

	}

	public class TokenStorage<TToken> where TToken : Token
	{
		public HashSet<TToken> Tokens { get; set; }

		public void Gc ( )
		{
			Tokens . RemoveWhere ( ( token ) => token . NotAfter < DateTimeOffset . UtcNow ) ;
		}

		public void AddToken( [NotNull] TToken token)
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if (token.NotAfter > DateTimeOffset.UtcNow)
			{
				Tokens . Add ( token ) ;
			}
			
		}

		public void DisposeToken ( [NotNull] TToken token )
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			Tokens.Remove(token);
		}

		public void CheckToken( [NotNull] TToken token)
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if (Tokens.Contains(token))
			{
				return;
			}
			else
			{
				throw new InvalidTokenException();
			}
		}
	}

}