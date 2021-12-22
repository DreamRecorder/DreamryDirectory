using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Anonymous : Entity
	{

		public override Guid Guid
		{
			get => DreamRecorder.Directory.Logic.KnownEntities.Anonymous;
			set => throw new InvalidOperationException ( ) ;
		}

	}

}
