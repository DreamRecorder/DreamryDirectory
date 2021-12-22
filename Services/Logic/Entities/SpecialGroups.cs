using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class SpecialGroups : SpecialGroup
	{
		public override Guid Guid
		{
			get => Guid . Parse ( "7C08B933-AE17-4879-9420-CFA209B9690A" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null )
			=> entity is SpecialGroup ;

	}

}
