using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . AspNetCore . Mvc ;
using Microsoft . Extensions . Logging ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ApiService . Controllers
{

	[ApiController]
	[Route ( "[controller]" )]
	public class WeatherForecastController : ControllerBase
	{

		private readonly ILogger <WeatherForecastController> _logger ;

		public WeatherForecastController ( ILogger <WeatherForecastController> logger ) { _logger = logger ; }

		private static readonly string [ ] Summaries =
		{
			"Freezing" ,
			"Bracing" ,
			"Chilly" ,
			"Cool" ,
			"Mild" ,
			"Warm" ,
			"Balmy" ,
			"Hot" ,
			"Sweltering" ,
			"Scorching"
		} ;

	}

}
