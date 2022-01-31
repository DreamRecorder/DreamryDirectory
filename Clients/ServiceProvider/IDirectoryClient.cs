using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Net ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public interface IDirectoryClient
		: IEntityTokenProvider , IDirectoryServiceProvider , ILoginService , IAccessTokenProvider
	{


		ICollection <EndPoint> Directories { get ; }

		string GetProperty ( Guid target , string name ) ;

	}

}
