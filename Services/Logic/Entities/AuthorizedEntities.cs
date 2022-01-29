using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class AuthorizedEntities : SpecialGroup
	{

		public override Guid Guid
		{
			get => KnownEntities . AuthorizedEntities ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null )
			=> entity is not Anonymous ;

	}

}
