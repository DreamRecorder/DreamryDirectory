using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic . Exceptions
{

	public abstract class TargetNotFoundException : Exception
	{

		public Guid TargetGuid { get ; set ; }

		public TargetNotFoundException ( Guid targetGuid ) => TargetGuid = targetGuid ;

	}

}
