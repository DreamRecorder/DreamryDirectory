using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . General ;

public class TokenStorageFactory:ITokenStorageFactory 
{

	public Type TargetType { get ; }

	public ITokenStorage <TToken> CreateTokenStorage <TToken> ( ) where TToken : Token =>( ITokenStorage <TToken> )Activator.CreateInstance( TargetType.MakeGenericType(typeof(TToken)))  ;

	public TokenStorageFactory ( [NotNull] Type targetType )
	{
		if (TargetType == null)
		{
			throw new ArgumentNullException(nameof(targetType));
		}

		if ( !targetType.IsGenericTypeDefinition)
		{
			throw new ArgumentException (nameof(targetType));
		}

		TargetType = targetType;
	}

}