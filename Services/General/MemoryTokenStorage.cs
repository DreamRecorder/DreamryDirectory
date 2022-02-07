using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Exceptions ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . General
{

	public class MemoryTokenStorage <TToken> : ITokenStorage <TToken> where TToken : Token
	{

		private ITaskDispatcher TaskDispatcher { get ; }

		private HashSet <TToken> Tokens { get ; } = new HashSet <TToken> ( ) ;

		private TimeSpan TotalLifetime { get ; set ; } = TimeSpan . Zero ;

		public MemoryTokenStorage ( ITaskDispatcher taskDispatcher )
		{
			TaskDispatcher = taskDispatcher ;

			TaskDispatcher . Dispatch ( new ScheduledTask ( Gc ) ) ;
		}

		public void AddToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if ( token . NotAfter > DateTimeOffset . UtcNow )
			{
				lock ( Tokens )
				{
					if ( Tokens . Add ( token ) )
					{
						TotalLifetime += token . NotAfter - DateTimeOffset . UtcNow ;
					}
				}
			}
		}

		public void DisposeToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			lock ( Tokens )
			{
				if ( Tokens . Remove ( token ) )
				{
					TotalLifetime = ( TotalLifetime / ( Tokens . Count + 1 ) ) * Tokens . Count ;
				}
			}
		}

		public void CheckToken ( [NotNull] TToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			lock ( Tokens )
			{
				if ( Tokens . Contains ( token ) )
				{
				}
				else
				{
					throw new InvalidTokenException ( ) ;
				}
			}
		}

		public DateTimeOffset ? Gc ( )
		{
			lock ( Tokens )
			{
				if ( Tokens . Count > 0 )
				{
					int count =
						Tokens . RemoveWhere (
											token => token . NotAfter < DateTimeOffset . UtcNow ) ;
					TotalLifetime =
						( TotalLifetime / ( Tokens . Count + count ) ) * Tokens . Count ;

					return DateTimeOffset . UtcNow + ( ( TotalLifetime / Tokens . Count ) / 2 ) ;
				}
				else
				{
					return DateTimeOffset . UtcNow + TimeSpan . FromMinutes ( 1 ) ;
				}
			}
		}

	}

}
