using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Storage ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class User : Entity
	{

		public DbUser DatabaseObject { get ; set ; }

	}

}
