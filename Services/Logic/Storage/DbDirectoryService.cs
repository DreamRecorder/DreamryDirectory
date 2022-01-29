﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel . DataAnnotations ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbEntity
	{

		[Required]
		public Guid Guid { get ; set ; }

		[Required]
		public DbEntityType EntityType { get ; set ; }

	}

}
