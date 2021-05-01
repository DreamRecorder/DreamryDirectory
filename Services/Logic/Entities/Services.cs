using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Services : SpecialGroup
	{

		public override HashSet<Entity> Members
			=> DirectoryServiceInternal.Current.DirectoryDatabase.Services.ToHashSet<Entity>();

		public override Guid Guid
		{
			get => Guid.Parse("C9CF393A-8969-4F7F-8851-AB1AA5192564");
			set => throw new InvalidOperationException();
		}

		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null)
		{
			return entity is Service;
		}

	}

}