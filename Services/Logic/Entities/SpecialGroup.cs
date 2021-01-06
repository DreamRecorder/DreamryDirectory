using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public abstract class SpecialGroup : Entity
	{

		public virtual HashSet <Entity> Members => throw new InvalidOperationException ( ) ;

	}

}
