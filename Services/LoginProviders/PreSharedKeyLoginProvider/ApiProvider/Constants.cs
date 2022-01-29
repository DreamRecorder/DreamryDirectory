using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ApiService ;

public static class Constants
{

	public static string Namespace
		=> "DreamRecorder.Directory.LoginProviders.PreSharedKeyLoginProvider" ;

	public static string EnabledPropertyName => "Enabled" . ToPropertyName ( ) ;

	public static string HashPropertyName => "Hash" . ToPropertyName ( ) ;

	public static string SaltPropertyName => "Salt" . ToPropertyName ( ) ;

	public static string HashVersion => "HashVersion" . ToPropertyName ( ) ;

	public static string ToPropertyName ( this string propertyName )
		=> $"{Namespace}.{propertyName}" ;

}
