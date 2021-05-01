using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . Services . General
{

	public class EntityNotFoundException : AuthenticationException
	{
		public Guid TargetGuid { get; set; }

		public Guid TokenIssuer { get ; set ; }


		public EntityNotFoundException(Token token)
		{
			TargetGuid  = token.Owner;
			TokenIssuer = token.Issuer;
		}
	}

	public abstract class TargetNotFoundException : Exception
	{
		public Guid TargetGuid { get; set; }

		public TargetNotFoundException ( Guid targetGuid ) { TargetGuid = targetGuid ; }

	}

}
