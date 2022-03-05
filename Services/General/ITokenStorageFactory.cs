using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . General ;

public interface ITokenStorageFactory
{

	ITokenStorage<TToken> CreateTokenStorage<TToken>() where TToken : Token;

}
