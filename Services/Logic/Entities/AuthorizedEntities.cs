using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class AuthorizedEntities : SpecialGroup
	{
		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) =>( (!(entity is null))&&(!(entity is Everyone)));

	}

}