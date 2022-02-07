using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class ClientLoginTokenProvider : ILoginTokenProvider
{

	public ICollection <(Guid loginProvider , object credential)> Credentials { get ; }


	public LoginToken GetToken ( ) => throw new NotImplementedException ( ) ;

}
