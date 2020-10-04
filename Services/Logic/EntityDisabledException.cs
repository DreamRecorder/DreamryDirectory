﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class EntityDisabledException : AuthenticationException
	{

		public Guid EntityGuid { get ; set ; }

		public EntityDisabledException ( Guid entityGuid ) { EntityGuid = entityGuid ; }

	}

}