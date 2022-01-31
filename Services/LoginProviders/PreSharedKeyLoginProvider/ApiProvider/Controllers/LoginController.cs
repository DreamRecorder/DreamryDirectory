using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . LoginProviders . ServiceProvider ;
using DreamRecorder . ToolBox . General ;

using Microsoft . AspNetCore . Mvc ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ApiService .
	Controllers
{

	[ApiController]
	public class LoginController : ControllerBase
	{

		public ILoginProvider LoginProvider { get ; }

		public LoginController ( ILoginProvider loginProvider ) => LoginProvider = loginProvider ;


		[HttpGet ( "Version" )]
		public ActionResult <string> Version ( )
			=> ProgramBase . Current . GetType ( ) . Assembly . GetDisplayName ( ) ;

		[HttpPost ( @"{loginServiceIdId:guid}/Login" )]
		public ActionResult <LoginToken> Login (
			Guid              loginServiceId ,
			[FromBody] object credential )
		{
			if ( LoginProvider . GetLoginService ( loginServiceId ) is ILoginService loginService )
			{
				return loginService . Login ( credential ) ;
			}

			return NotFound ( ) ;
		}

		[HttpPost ( @"{loginServiceIdId:guid}/CheckToken" )]
		public ActionResult CheckToken (
			Guid                     loginServiceId ,
			[FromHeader] AccessToken token ,
			[FromBody]   LoginToken  tokenToCheck )
		{
			if ( LoginProvider . GetLoginService ( loginServiceId ) is ILoginService loginService )
			{
				loginService . CheckToken ( token , tokenToCheck ) ;
				return Ok ( ) ;
			}

			return NotFound ( ) ;
		}

		[HttpPost ( @"{loginServiceIdId:guid}/DisposeToken" )]
		public ActionResult DisposeToken ( Guid loginServiceId , [FromBody] LoginToken token )
		{
			if ( LoginProvider . GetLoginService ( loginServiceId ) is ILoginService loginService )
			{
				loginService . DisposeToken ( token ) ;
				return Ok ( ) ;
			}

			return NotFound ( ) ;
		}

	}

}
