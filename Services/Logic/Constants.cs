using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class Constants
	{

		public static string Namespace => KnownNamespaces . DirectoryServicesNamespace ;

		public static string ToPropertyName ( this string propertyName )
			=> $"{Namespace}.{propertyName}" ;

	}

}
