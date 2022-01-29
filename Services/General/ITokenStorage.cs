using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . General ;

public interface ITokenStorage <TToken> where TToken : Token
{

	void AddToken ( [NotNull] TToken token ) ;

	void DisposeToken ( [NotNull] TToken token ) ;

	void CheckToken ( [NotNull] TToken token ) ;

}
