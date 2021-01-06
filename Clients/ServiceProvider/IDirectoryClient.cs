using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Net ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface IDirectoryClient : IEntityTokenProvider , ILoginProvider , IAccessTokenProvider
	{

		ICollection <(ILoginProvider loginProvider , object credential)> Credentials { get ; }

		ICollection <EndPoint> Directories { get ; }

	}

}
