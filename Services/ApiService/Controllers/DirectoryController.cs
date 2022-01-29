using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . ToolBox . General ;

using Microsoft . AspNetCore . Mvc ;
using Microsoft . Extensions . Logging ;

namespace DreamRecorder . Directory . Services . ApiService . Controllers
{

	[ApiController]
	public class DirectoryController : ControllerBase
	{

		private readonly ILogger <DirectoryController> _logger ;

		private IDirectoryService DirectoryService { get ; }

		public DirectoryController (
			ILogger <DirectoryController> logger ,
			IDirectoryService             directoryService )
		{
			_logger          = logger ;
			DirectoryService = directoryService ;
		}

		[HttpGet ( "Version" )]
		public ActionResult <string> Version ( )
			=> ProgramBase . Current . GetType ( ) . Assembly . GetDisplayName ( )
			+ Environment . NewLine
			+ DirectoryService . GetType ( ) . Assembly . GetDisplayName ( ) ;

		[HttpPost ( "Login" )]
		public ActionResult <EntityToken> Login ( [FromBody] LoginToken token )
		{
			if ( token is null )
			{
				return BadRequest ( ) ;
			}

			return DirectoryService . Login ( token ) ;
		}

		[HttpPost ( @"DisposeToken" )]
		public ActionResult DisposeToken ( [FromBody] EntityToken token )
		{
			if ( token is null )
			{
				return BadRequest ( ) ;
			}

			DirectoryService . DisposeToken ( token ) ;

			return Ok ( ) ;
		}

		[HttpPost ( @"UpdateToken" )]
		public ActionResult <EntityToken> UpdateToken ( [FromHeader] EntityToken token )
		{
			if ( token is null )
			{
				return BadRequest ( ) ;
			}

			return DirectoryService . UpdateToken ( token ) ;
		}


		[HttpPost ( "SetProperty/{target:guid}/{name}" )]
		public ActionResult SetProperty (
			[FromHeader] EntityToken token ,
			Guid                     target ,
			string                   name ,
			[FromBody] string        value )
		{
			if ( token is null )
			{
				return BadRequest ( ) ;
			}

			DirectoryService . SetProperty ( token , target , name , value ) ;

			return Ok ( ) ;
		}

	}

}
