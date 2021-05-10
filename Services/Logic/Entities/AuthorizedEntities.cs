using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class AuthorizedEntities : SpecialGroup
	{

		public override Guid Guid
		{
			get => Guid . Parse ( "D7A1809A-2E3B-4BCD-8927-3E2D96751F1F" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Entity> checkedEntities = null )
			=> ( ( ! ( entity is null ) ) && ( ! ( entity is Anonymous ) ) ) ;

	}

}
