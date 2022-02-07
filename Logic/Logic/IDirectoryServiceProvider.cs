using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic
{

	public interface IDirectoryServiceProvider
	{

		IDirectoryService GetDirectoryService ( ) ;

	}

	public interface ILoginServiceProvider
	{

		ILoginService GetLoginService ( Guid type ) ;

	}

}
