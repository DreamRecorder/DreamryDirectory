using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Logic ;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.Controllers
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



        [HttpGet]
        public EntityToken Login(LoginToken token)
        {
           
        }
    }
}
