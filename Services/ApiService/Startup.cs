using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic ;
using DreamRecorder . Directory . Services . Logic . Storage ;
using DreamRecorder . ToolBox . AspNet . General ;

using Microsoft . AspNetCore . Builder ;
using Microsoft . AspNetCore . Hosting ;
using Microsoft . Extensions . Configuration ;
using Microsoft . Extensions . DependencyInjection ;
using Microsoft . Extensions . Hosting ;

namespace DreamRecorder . Directory . Services . ApiService
{

	public class Startup
	{

		public IConfiguration Configuration { get ; }

		public Startup ( IConfiguration configuration ) => Configuration = configuration ;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices ( IServiceCollection services )
		{
			services . AddControllers (
										HeaderComplexModelBinder .
											EnableHeaderComplexModelBinder ( ) ) ;

			services . AddDbContext <DirectoryDatabaseStorage> ( ) ;


			//options
			//	=> options.UseSqlServer(
			//							Configuration.GetConnectionString(
			//															nameof(DirectoryDatabaseStorage)))

			services . AddSingleton <IDirectoryService , DirectoryServiceBase> ( ) ;
			services . AddSingleton <IDirectoryDatabaseStorage , DirectoryDatabaseStorage> ( ) ;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure ( IApplicationBuilder app , IWebHostEnvironment env )
		{
			if ( env . IsDevelopment ( ) )
			{
				app . UseDeveloperExceptionPage ( ) ;
			}

			app . UseHttpsRedirection ( ) ;

			app . UseRouting ( ) ;

			app . UseEndpoints ( endpoints => { endpoints . MapControllers ( ) ; } ) ;
		}

	}

}
