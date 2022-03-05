using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;

namespace DreamRecorder . Directory . LoginProviders . ServiceProvider ;

public class LoginServiceLoginTokenProvider:ILoginTokenProvider
{

	public LoginServiceLoginTokenProvider (LoginServiceBase loginService , Guid target )
	{
		LoginService = loginService ;
		Target       = target ;
	}

	private LoginServiceBase LoginService { get; }

	public Guid Target { get;}

	public LoginToken GetToken ( ) => LoginService.IssueLoginToken(Target) ;

}