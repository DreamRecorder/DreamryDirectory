using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic.Tokens;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DreamRecorder.Directory.ApiService.Controllers
{

	[ApiController]
    [Route("[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly ILogger<DirectoryController> _logger;

        public DirectoryController(ILogger<DirectoryController> logger)
        {
            _logger = logger;
        }



		[HttpPost ( )]
		public EntityToken Login ( )
		{
            Request.Body

		}



    }
}
