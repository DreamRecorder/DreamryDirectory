using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic ;

[AttributeUsage ( AttributeTargets . Property )]
public class LoginTypeAttribute : Attribute
{

	public virtual Type CredentialType { get ; }

	public LoginTypeAttribute ( Type credentialType ) => CredentialType = credentialType ;

	protected LoginTypeAttribute ( ) { }

}

public static class KnownLoginType
{

	[LoginType ( typeof ( PreSharedKeyCredential ) )]
	public static Guid PreSharedKey => Guid . Parse ( "58BD0A39-3F39-485F-9BC7-18642A48F85D" ) ;

}
