using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Users : SpecialGroup
	{

		public override HashSet<Entity> Members
			=> DirectoryServiceInternal.Current.DirectoryDatabase.Users.ToHashSet<Entity>();

		public override Guid Guid
		{
			get => Guid.Parse("80F95DEE-A591-4E41-9747-0ECE02585075");
			set => throw new InvalidOperationException();
		}

		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null)
		{
			return entity is Users;
		}

	}

}
