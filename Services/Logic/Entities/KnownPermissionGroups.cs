using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Permissions ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public static class KnownPermissionGroups
	{

		public static PermissionGroup InternalApiOnly
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . InternalApiOnly ) ;

		public static PermissionGroup EveryoneReadonly
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . EveryoneReadonly ) ;

		public static PermissionGroup EveryoneReadWrite
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . EveryoneReadWrite ) ;

		public static PermissionGroup AuthorizedReadonly
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . AuthorizedReadonly ) ;

		public static PermissionGroup AuthorizedReadWrite
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . AuthorizedReadWrite ) ;

		public static PermissionGroup LoginServicesReadonly
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . LoginServicesReadonly ) ;

		public static PermissionGroup LoginServicesReadWrite
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . LoginServicesReadWrite ) ;

		public static PermissionGroup DirectoryServicesReadonly
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . DirectoryServicesReadonly ) ;

		public static PermissionGroup DirectoryServicesReadWrite
			=> DirectoryServiceInternal . Current . DirectoryDatabase . FindPermissionGroup (
			Directory . Logic . KnownPermissionGroups . DirectoryServicesReadWrite ) ;

	}

}
