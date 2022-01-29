using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public class PreSharedKeyCredential
	{

		public Guid Target { get ; set ; }

		public string PreSharedKey { get ; set ; }

	}

}
