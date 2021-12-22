using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class DirectoryServiceInternal
	{

		public static IDirectoryServiceInternal Current { get ; set ; }

		public static Entity FindEntity(Guid guid)=>Current.DirectoryDatabase.FindEntity(guid);

		public static KnownSpecialGroups KnownSpecialGroups => Current.DirectoryDatabase.KnownSpecialGroups;

	}

}
