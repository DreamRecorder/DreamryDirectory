using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ServiceProvider
{

	public class PreSharedKeyLoginService : LoginServiceBase <PreSharedKeyCredential>
	{

		public override Guid ? CheckCredential ( PreSharedKeyCredential credential ) { }

	}

}
