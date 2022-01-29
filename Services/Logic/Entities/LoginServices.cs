using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class LoginServices : SpecialGroup
	{

		public override Guid Guid
		{
			get => Guid . Parse ( "162e0359-7a24-41d0-bc92-d627f51b9ae9" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null )
			=> entity is LoginService ;

	}

}
