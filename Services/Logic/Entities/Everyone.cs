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
			get => Guid . Parse ( "CCAD71E9-041B-4190-AE0D-1B7034FF2E15" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Entity> checkedEntities = null ) => true ;

	}

}
