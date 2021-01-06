using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Net . Http ;
using System . Text . Json ;

using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public class RemoteLoginProvider <TCredential> : ILoginProvider where TCredential : class
	{

		public string Server { get ; set ; }

		public int Port { get ; set ; }

		public LoginToken Login ( object credential ) { return Login ( credential as TCredential ) ; }

		public void CheckToken ( AccessToken token , LoginToken tokenToCheck )
		{
			HttpClient client = new HttpClient ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( CheckToken ) ) . Uri ,
																	tokenToCheck ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public virtual LoginToken Login ( TCredential credential )
		{
			HttpClient client = new HttpClient ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( Login ) ) . Uri ,
																	credential ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			LoginToken result = response . Content . ReadAsAsync <LoginToken> ( ) . Result ;

			return result ;
		}

	}

}
