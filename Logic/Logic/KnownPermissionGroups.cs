using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic
{

	public static class KnownPermissionGroups
	{

		public static Guid InternalApiOnly => Guid . Parse ( "fa3f35a3-22bb-4033-b7ef-3b051386a8a1" ) ;

		public static Guid EveryoneReadonly => Guid . Parse ( "fb45a7d6-293d-4ce5-b11b-21749f2fc67d" ) ;

		public static Guid EveryoneReadWrite => Guid . Parse ( "2ef65ab5-80b9-47d3-9818-2da6f475fa07" ) ;

		public static Guid AuthorizedReadonly => Guid . Parse ( "30d2b940-70bf-4777-a90a-1577ec8b816f" ) ;

		public static Guid AuthorizedReadWrite => Guid . Parse ( "89e2d48a-c94b-45dd-b35a-eab09f0445ba" ) ;

		public static Guid LoginServicesReadonly => Guid . Parse ( "fdff13ed-fe7e-4a74-be9d-edb223a861a0" ) ;

		public static Guid LoginServicesReadWrite => Guid . Parse ( "0ce5f9fb-505b-471d-bd88-18a453c16e85" ) ;

		public static Guid DirectoryServicesReadonly => Guid . Parse ( "bb85a887-67bc-466e-b231-be5bbf09472f" ) ;

		public static Guid DirectoryServicesReadWrite => Guid . Parse ( "1479e35c-bcba-47ad-b850-b51d3877a263" ) ;

	}

	public static class KnownGroups
	{
	}

}
