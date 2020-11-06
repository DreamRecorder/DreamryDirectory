using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public interface IDirectoryDatabase
	{

		HashSet <User> Users { get ; }

		HashSet <Group> Groups { get ; set ; }

		HashSet <Service> Services { get ; set ; }

		HashSet <LoginService> LoginServices { get ; set ; }

		HashSet <DirectoryService> DirectoryServices { get ; set ; }

	}

}