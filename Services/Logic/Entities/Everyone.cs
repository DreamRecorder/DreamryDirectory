﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Everyone : SpecialGroup
	{
		public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null) => true;

	}

}