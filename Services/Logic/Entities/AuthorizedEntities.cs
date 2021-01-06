﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class AuthorizedEntities : SpecialGroup
	{

		public override Guid Guid
		{
			get => Guid . Parse ( "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF" ) ;
			set => throw new InvalidOperationException ( ) ;
		}

		public override bool Contain ( Entity entity , HashSet <Entity> checkedEntities = null )
		{
			return ( ( ! ( entity is null ) ) && ( ! ( entity is Everyone ) ) ) ;
		}

	}

}
