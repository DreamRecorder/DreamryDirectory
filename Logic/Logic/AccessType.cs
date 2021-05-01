using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DreamRecorder.Directory.Logic
{

	[Flags]
	public enum AccessType
	{

		ReadWrite = 3,

		Write = 2,

		Read = 1,

		None = 0,

	}

}
