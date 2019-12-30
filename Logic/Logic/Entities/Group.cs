using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic . Entities
{

	public class Group : Entity
	{

		public HashSet<Entity> Members { get; set; }

		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null)
		{
			checkedEntities ??= new HashSet<Entity>();

			if (checkedEntities.Contains(this))
			{
				return base.Contain(entity, checkedEntities);
			}

			else
			{
				if (base.Contain(entity, checkedEntities))
				{
					return true;
				}
				else
				{
					foreach (Entity member in Members.Except(checkedEntities))
					{
						if (member.Contain(entity, checkedEntities))
						{
							return true;
						}
					}
				}

				return false;
			}
		}

	}

}