using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;
using System . Reflection ;

namespace DreamRecorder . Directory . Logic ;

public static class KnownProperties
{

	public static ReadOnlyDictionary <string , string> PropertiesList { get ; }

	static KnownProperties ( )
	{
		Dictionary <string , string> dictionary = typeof ( KnownProperties ) .
												GetFields (
															BindingFlags . Static
														| BindingFlags . Public
														| BindingFlags . DeclaredOnly ) .
												ToDictionary (
															fieldInfo => fieldInfo . Name ,
															fieldInfo
																=> ( ( string )
																		fieldInfo . GetValue (
																		null ) ) .
																ToPropertyName (
																fieldInfo .
																	GetCustomAttribute <
																		PropertyAttribute> ( ) .
																	Namespace ) ) ;

		PropertiesList = new ReadOnlyDictionary <string , string> ( dictionary ) ;
	}

	[Property ( EntityScope . Any , KnownNamespaces . DirectoryServicesNamespace , false )]
	public const string DisplayName = nameof ( DisplayName ) ;

	[Property ( EntityScope . Any , KnownNamespaces . DirectoryServicesNamespace , true )]
	public const string IsDisabled = nameof ( IsDisabled ) ;

	[Property( EntityScope.LoginService, KnownNamespaces.DirectoryServicesNamespace, true)]
	public const string LoginType = nameof(LoginType);


	[Property (
				EntityScope . DirectoryService | EntityScope . LoginService ,
				KnownNamespaces . DirectoryServicesNamespace ,
				true )]
	public const string ApiEndpoints = nameof ( ApiEndpoints ) ;


	public static ReadOnlyCollection <(string HostName , int Port)> ParseApiEndpoints (
		string value )
	{
		List <(string HostName , int Port)> result = new List <(string HostName , int Port)> ( ) ;

		List <string> endPointsStrings = value .
										Split ( ',' , StringSplitOptions . RemoveEmptyEntries ) .
										Select ( str => str . Trim ( ) ) .
										ToList ( ) ;


		foreach ( string endPointsString in endPointsStrings )
		{
			string [ ] endPointsParts = endPointsString .
										Split ( ':' , StringSplitOptions . RemoveEmptyEntries ) .
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

		return new ReadOnlyCollection <(string HostName , int Port)> ( ( result ) ) ;
	}

	public static string ToPropertyName ( this string propertyName , string @namespace )
		=> $"{@namespace}.{propertyName}" ;


	public static string GetPropertyName ( this string propertyName )
		=> PropertiesList [ propertyName ] ;

}
