using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public abstract class SpecialGroup : Entity
	{




	}

	public class SpecialGroups : SpecialGroup
	{

		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) => entity is SpecialGroup;

	}

	public class KnownSpecialGroups
	{
		public Everyone Everyone{ get; set; }

		public AuthorizedEntities AuthorizedEntities{ get; set; }

		public SpecialGroups SpecialGroups { get ; set ; }

		public DirectoryServices DirectoryServices{ get; set; }

		public KnownSpecialGroups ( ) {

		}

		public List <SpecialGroup> Entities
			=> typeof ( KnownSpecialGroups ) . GetProperties ( ) .
												Where (
														prop
															=> typeof ( SpecialGroup ) . IsAssignableFrom (
															prop . PropertyType ) ).Select(prop=>prop.GetValue(this) as SpecialGroup) .
												ToList ( ) ;

	}

	public class Everyone : SpecialGroup
	{
		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) => true;

	}

}