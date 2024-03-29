using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . AspNetCore . Builder ;
using Microsoft . AspNetCore . Hosting ;
using Microsoft . Extensions . Configuration ;
using Microsoft . Extensions . DependencyInjection ;
using Microsoft . Extensions . Hosting ;

namespace DreamRecorder . Directory . LoginProviders . PreSharedKeyLoginProvider . ApiService
{

	public class Startup
	{

		public IConfiguration Configuration { get ; }

		public Startup ( IConfiguration configuration ) => Configuration = configuration ;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices ( IServiceCollection services )
		{
			services . AddControllers ( ) ;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure ( IApplicationBuilder app , IWebHostEnvironment env )
		{
			if ( env . IsDevelopment ( ) )
			{
				app . UseDeveloperExceptionPage ( ) ;
			}

			app . UseHttpsRedirection ( ) ;
			app . UseStaticFiles ( ) ;

			app . UseRouting ( ) ;

			app . UseAuthorization ( ) ;

			app . UseEndpoints ( endpoints => { endpoints . MapControllers ( ) ; } ) ;
		}

	}

}
