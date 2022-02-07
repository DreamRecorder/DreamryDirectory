using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . Logic
{

	public interface ILoginService : IService
	{

		LoginToken Login ( object credential ) ;

		void CheckToken ( AccessToken token , LoginToken tokenToCheck ) ;

		void DisposeToken ( LoginToken token ) ;

	}

	public abstract class ServiceBase : IService
	{

		public DateTimeOffset ? StartupTime { get ; private set ; }

		public DateTimeOffset GetStartupTime ( ) => StartupTime ?? DateTimeOffset . Now ;

		public DateTimeOffset GetTime ( ) => DateTime . Now ;

		public virtual Version GetVersion ( ) => GetType ( ) . Assembly . GetName ( ) . Version ;

		public virtual void Start ( ) { StartupTime ??= DateTimeOffset . Now ; }

	}

}
