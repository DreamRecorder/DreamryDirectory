﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder.Directory.Logic.Entities ;

namespace DreamRecorder . Directory . Logic
{

	public interface ILoginServiceProvider
	{

		ILoginProvider GetLoginProvider ( LoginProvider loginProvider ) ;

	}

}