﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . Logic . Exceptions
{

	public class EntityNotFoundException : AuthenticationException
	{

		public Guid TargetGuid { get ; set ; }

		public Guid TokenIssuer { get ; set ; }


		public EntityNotFoundException ( Token token )
		{
			TargetGuid  = token . Owner ;
			TokenIssuer = token . Issuer ;
		}

	}

}
