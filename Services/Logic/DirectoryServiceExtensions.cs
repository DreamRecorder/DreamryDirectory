using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

using static DreamRecorder . Directory . Logic . KnownProperties ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class DirectoryServiceExtensions
	{

		public static readonly string
			DatabaseConnectionString = nameof ( DatabaseConnectionString ) ;

		public static readonly string DatabaseConnectionStringName =
			DatabaseConnectionString . ToPropertyName ( ) ;

		public static readonly string ApiEndpointsName = ApiEndpoints . ToPropertyName ( ) ;

		public static EntityProperty GetDatabaseConnectionStringProperty (
			[NotNull] this DirectoryService directoryService )
		{
			if ( directoryService == null )
			{
				throw new ArgumentNullException ( nameof ( directoryService ) ) ;
			}

			return directoryService . GetOrCreateProperty ( DatabaseConnectionStringName ) ;
		}

		public static EntityProperty GetApiEndPointsProperty (
			[NotNull] this DirectoryService directoryService )
		{
			if ( directoryService == null )
			{
				throw new ArgumentNullException ( nameof ( directoryService ) ) ;
			}

			return directoryService . GetOrCreateProperty (
															ApiEndpointsName ,
															DirectoryServiceInternal .
																KnownSpecialGroups .
																DirectoryServices . Guid ,
															KnownPermissionGroups .
																EveryoneReadonly ) ;
		}

		public static ReadOnlyCollection <(string HostName , int Port)> GetApiEndpoints (
			[NotNull] this DirectoryService directoryService )
		{
			if ( directoryService == null )
			{
				throw new ArgumentNullException ( nameof ( directoryService ) ) ;
			}

			return ParseApiEndpoints ( directoryService . GetApiEndPointsProperty ( ) . Value ) ;
		}

	}

}
