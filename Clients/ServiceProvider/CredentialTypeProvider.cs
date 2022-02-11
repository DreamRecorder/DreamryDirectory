using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . ServiceProvider ;

public class CredentialTypeProvider : ICredentialTypeProvider
{

	private static Dictionary <Guid , Type> credentialTypes ;

	[Prepare]
	public static void Prepare ( )
	{
		lock ( StaticServiceProvider . ServiceCollection )
		{
			credentialTypes = AppDomainExtensions .
							FindProperty <LoginTypeAttribute> (
																( prop )
																	=> prop . info . PropertyType == typeof ( Guid )
																		&& prop . info . GetAccessors ( ) .
																			First ( ) .
																			IsStatic ) .
							Select (
									prop => ( ( Guid )prop . info . GetValue ( null ) ,
											prop . attribute . CredentialType ) ) .
							ToDictionary ( tuple => tuple . Item1 , tuple => tuple . CredentialType ) ;
		}
	}

	public Type GetCredentialType ( Guid loginType ) => credentialTypes [ loginType ] ;

}