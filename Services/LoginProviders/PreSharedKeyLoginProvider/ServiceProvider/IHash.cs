﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . LoginProviders . ServiceProvider ;

public interface IHash
{

	Guid Guid { get ; }

	byte [ ] Hash ( string password , string salt ) ;

}
