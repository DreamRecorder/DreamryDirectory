using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System . Net ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface IDirectoryClient:ITokenProvider,ILoginProvider,IAccessTokenProvider
	{
		ICollection<(ILoginProvider loginProvider,object credential)> Credentials { get; }

		ICollection<EndPoint> Directories { get ; }
	}

}