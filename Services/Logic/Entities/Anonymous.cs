using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Anonymous : Entity
	{

		public override Guid Guid
		{
			get => KnownEntities . Anonymous ;
			set => throw new InvalidOperationException ( ) ;
		}

	}

}
