using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . LoginProviders . ServiceProvider ;

public interface ILoginProvider
{

	[CanBeNull]
	ILoginService GetLoginService ( Guid guid ) ;

}
