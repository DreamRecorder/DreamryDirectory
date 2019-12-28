using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder.Directory.Logic ;
using DreamRecorder.Directory.Logic.Entities ;
using DreamRecorder.Directory.Logic.Tokens ;

using JetBrains . Annotations ;

using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace DreamRecorder . Directory . ApiService .Controllers
{

	
	public class DirectoryService : IDirectoryProvider
	{

       public HashSet<User> Users { get; }

		public HashSet<Group> Groups { get ; set ; }

        public HashSet<Service> Services { get ; set ; }

		public HashSet<LoginProvider> LoginProviders { get; set; }

		public HashSet<DirectoryService> DirectoryServices { get; set; }

        public Everyone Everyone { get; set; }

        public AuthorizedUser AuthorizedUser { get; set; }



        public HashSet<EntityToken> IssuedEntityTokens { get; set; }

        public HashSet<AccessToken> IssuedAccessTokens { get; set; }

		public bool CheckTokenTime( [NotNull] Token token)
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			return token . NotAfter      > DateTimeOffset . UtcNow
					&& token . NotBefore < DateTimeOffset . UtcNow ;
		}


		public EntityToken Login ( [NotNull] LoginToken token )
		{
			if ( token == null )
			{
				throw new ArgumentNullException ( nameof ( token ) ) ;
			}

			if (CheckTokenTime(token))
			{
				
			}



		}

		public EntityToken ChangeToken ( EntityToken token , Guid target ) => throw new NotImplementedException ( ) ;

		public AccessToken Access ( EntityToken token , Guid target ) => throw new NotImplementedException ( ) ;

		public string GetProperty ( EntityToken token , Guid target , string name ) => throw new NotImplementedException ( ) ;

		public void SetProperty ( EntityToken token , Guid target , string name , string value ) { throw new NotImplementedException ( ) ; }

		public AccessType AccessProperty ( EntityToken token , Guid target , string name ) => throw new NotImplementedException ( ) ;

		public bool Contain ( EntityToken token , Guid @group , Guid user ) => throw new NotImplementedException ( ) ;

		public ICollection <Guid> ListGroup ( EntityToken token , Guid @group ) => throw new NotImplementedException ( ) ;

		public void AddToGroup ( EntityToken token , Guid @group , Guid target ) { throw new NotImplementedException ( ) ; }

		public void RemoveFromGroup ( EntityToken token , Guid @group , Guid target ) { throw new NotImplementedException ( ) ; }

		public bool CheckToken ( EntityToken token , AccessToken tokenToCheck ) => throw new NotImplementedException ( ) ;

		public Guid CreateUser ( EntityToken token ) => throw new NotImplementedException ( ) ;

		public Guid CreateGroup ( EntityToken token ) => throw new NotImplementedException ( ) ;

		public void RegisterLogin ( EntityToken loginServiceToken , EntityToken targetToken ) { throw new NotImplementedException ( ) ; }

	}

    [ApiController]
    [Route("[controller]")]
    public class DirectoryController : ControllerBase
    {
		private readonly ILogger<DirectoryController> _logger;

        public DirectoryController(ILogger<DirectoryController> logger)
        {
            _logger = logger;
        }



        [HttpPost()]
        public EntityToken Login(LoginToken token) { throw new Exception ( ) ; }

		public AccessToken Access ( EntityToken token , Guid target )
		{
			throw new Exception();
        }
    }
}
