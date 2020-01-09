using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic . Entities
{

	public class AuthorizedUser : Entity
	{
		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) =>( (!(entity is null))&&(!(entity is Everyone)));

	}

}