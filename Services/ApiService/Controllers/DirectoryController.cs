using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System . Reflection . Metadata ;
using System . Text . Json ;
using System . Threading . Tasks ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder.Directory.Logic.Tokens ;

using Microsoft.AspNetCore.Mvc ;
using Microsoft . AspNetCore . Mvc . ModelBinding ;
using Microsoft . AspNetCore . Mvc . ModelBinding . Binders ;
using Microsoft . AspNetCore . Mvc . ModelBinding . Metadata ;
using Microsoft.Extensions.Logging ;

namespace DreamRecorder . Directory . Services . ApiService .Controllers
{
	public class HeaderComplexModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			string headerKey = bindingContext.ModelMetadata.BinderModelName??bindingContext.FieldName;

			if ( ! string . IsNullOrEmpty ( headerKey ) )
			{
				string headerValue =
					bindingContext . HttpContext . Request . Headers [ headerKey ] . FirstOrDefault ( ) ;

				if ( ! string . IsNullOrEmpty ( headerValue ) )
				{
					Type modelType = bindingContext.ModelMetadata.ModelType;

					bindingContext. Model  = JsonSerializer . Deserialize ( headerValue , modelType ) ;
					bindingContext . Result = ModelBindingResult . Success ( bindingContext . Model ) ;

					return Task.CompletedTask;
				}
			}

			bindingContext.Result = ModelBindingResult.Failed();

			return Task.CompletedTask;
		}
	}

	public class HeaderComplexModelBinderProvider : IModelBinderProvider
	{

		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (context.Metadata.IsComplexType)
			{
				if ( context.Metadata is DefaultModelMetadata metadata )
				{
					object headerAttribute = metadata.Attributes.Attributes.FirstOrDefault( a => a.GetType() == typeof(FromHeaderAttribute) );

					if (headerAttribute != null)
					{
						return new BinderTypeModelBinder(typeof(HeaderComplexModelBinder));
					}
				}
			}
	
			return null;
		}
	}

	[ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly ILogger<DirectoryController> _logger;

		private IDirectoryService DirectoryService { get; }

        public DirectoryController(ILogger<DirectoryController> logger , IDirectoryService directoryService )
		{
			_logger          = logger;
			DirectoryService = directoryService ;
		}

		[HttpPost(@"Login")]
		public ActionResult<EntityToken> Login ([FromBody] LoginToken token )
		{
			if ( token is null )
			{
				return BadRequest ( ) ;
			}

			return DirectoryService . Login ( token ) ;
		}

		[HttpPost(@"DisposeToken")]
		public ActionResult DisposeToken([FromBody] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.DisposeToken(token);

			return Ok ( ) ;
		}

		[HttpPost(@"UpdateToken")]
		public ActionResult<EntityToken> UpdateToken([FromHeader] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.UpdateToken(token);
		}





		[HttpPost(@"SetProperty/{target:guid}/{name}")]
		public ActionResult SetProperty([FromHeader] EntityToken token, Guid target, string name,[FromBody] string value)
		{
			return Ok ( ) ;
		}


	}
}
