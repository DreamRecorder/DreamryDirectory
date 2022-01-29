﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage ;

public enum DbEntityType : byte
{

	Anonymous = 0 ,

	User = 1 ,

	DirectoryService = 2 ,

	LoginService = 3 ,

	Group = 4 ,

	Service = 5 ,

	SpecialGroup = 6 ,

}
