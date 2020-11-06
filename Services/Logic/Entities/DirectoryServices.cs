using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class DirectoryServices : SpecialGroup
	{
		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) => (entity is DirectoryService);

		public override Guid Guid { get => Guid.Parse("020e0fc0-01af-470f-85ed-c1b497713c35"); set => throw new InvalidOperationException(); }
	}

}