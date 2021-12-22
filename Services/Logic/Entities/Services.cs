using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Services : SpecialGroup
	{
		public override Guid Guid
		{
			get => DreamRecorder.Directory.Logic.KnownEntities.Services;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null ) => entity is Service ;

	}

}
