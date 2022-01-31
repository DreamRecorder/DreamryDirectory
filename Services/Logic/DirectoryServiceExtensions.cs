using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

using KnownPermissionGroups = DreamRecorder . Directory . Services . Logic . Entities . KnownPermissionGroups ;

using static DreamRecorder.Directory.Logic.KnownProperties;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class DirectoryServiceExtensions
	{

		public static readonly string
			DatabaseConnectionString = nameof ( DatabaseConnectionString ) ;

		public static readonly string DatabaseConnectionStringName =
			DatabaseConnectionString.ToPropertyName() ;

		public static readonly string ApiEndpointsName =ApiEndpoints.ToPropertyName() ;

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

		public static List <(string HostName , int Port)> GetApiEndpoints (
			[NotNull] this DirectoryService directoryService )
		{
			if ( directoryService == null )
			{
				throw new ArgumentNullException ( nameof ( directoryService ) ) ;
			}

			List <(string HostName , int Port)>
				result = new List <(string HostName , int Port)> ( ) ;

			List <string> endPointsStrings = directoryService . GetApiEndPointsProperty ( ) .
																Value .
																Split (
																',' ,
																StringSplitOptions .
																	RemoveEmptyEntries ) .
																Select ( str => str . Trim ( ) ) .
																ToList ( ) ;


			foreach ( string endPointsString in endPointsStrings )
			{
				string [ ] endPointsParts = endPointsString .
											Split (
													':' ,
													StringSplitOptions . RemoveEmptyEntries ) .
											Select ( str => str . Trim ( ) ) .
											ToArray ( ) ;

				if ( string . IsNullOrEmpty ( endPointsParts . FirstOrDefault ( ) ) )
				{
					string hostname = endPointsParts [ 0 ] ;

					if ( endPointsParts . Length == 2 )
					{
						result . Add ( ( hostname , int . Parse ( endPointsParts [ 1 ] ) ) ) ;
					}
					else
					{
						result . Add ( ( hostname , 443 ) ) ;
					}
				}
			}

			return result ;
		}

	}

}
