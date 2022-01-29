using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Everyone : SpecialGroup
	{

		public override Guid Guid
		{
			get => KnownEntities . Everyone ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null )
			=> true ;

	}

}
