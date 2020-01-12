using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public interface ILoginServiceProvider
	{

		ILoginProvider GetLoginProvider ( LoginProvider loginProvider ) ;

	}

}