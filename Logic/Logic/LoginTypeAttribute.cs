using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic ;

[AttributeUsage ( AttributeTargets . Property )]
public class LoginTypeAttribute : Attribute
{

	public virtual Type CredentialType { get ; }

	public LoginTypeAttribute ( Type credentialType ) => CredentialType = credentialType ;

	protected LoginTypeAttribute ( ) { }

}