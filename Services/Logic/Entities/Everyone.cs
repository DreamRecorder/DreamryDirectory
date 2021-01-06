using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Everyone : SpecialGroup
	{

		public override Guid Guid
		{
			get => Guid . Parse ( "00000000-0000-0000-0000-000000000000" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Entity> checkedEntities = null ) { return true ; }

	}

}
