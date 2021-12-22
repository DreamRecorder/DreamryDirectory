using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Groups : SpecialGroup
	{

		public override Guid Guid
		{
			get => Guid . Parse ( "E02BFEA6-EF75-405E-92BE-3141DDC200F1" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null ) => entity is Group ;

	}

}
