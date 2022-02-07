using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Net . Http ;
using System . Text . Json ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Logic . Tokens ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public abstract class RemoteServiceBase
	{

		public Func <HttpClient> HttpClientFactory { get ; set ; } = ( ) => new HttpClient ( ) ;

		public string HostName { get ; }

		public int Port { get ; }


		protected RemoteServiceBase ( string hostName , int port )
		{
			HostName = hostName ;
			Port     = port ;
		}

		public abstract TimeSpan MeasureLatency ( ) ;


		public DateTimeOffset GetStartupTime ( )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																	Uri . UriSchemeHttps ,
																	HostName ,
																	Port ,
																	nameof ( GetStartupTime ) ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			DateTimeOffset result = response . Content . ReadAsAsync <DateTimeOffset> ( ) . Result ;

			return result ;
		}

		public DateTimeOffset GetTime ( )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																Uri . UriSchemeHttps ,
																HostName ,
																Port ,
																nameof ( GetTime ) ) . Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			DateTimeOffset result = response . Content . ReadAsAsync <DateTimeOffset> ( ) . Result ;

			return result ;
		}

		public Version GetVersion ( )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																Uri . UriSchemeHttps ,
																HostName ,
																Port ,
																nameof ( GetVersion ) ) . Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			Version result = response . Content . ReadAsAsync <Version> ( ) . Result ;

			return result ;
		}

	}

	public abstract class RemoteLoginService : RemoteServiceBase , ILoginService
	{
		public Guid Type { get; private set; }

		public RemoteLoginService((string HostName, int Port) server) : base(
		server.HostName,
		server.Port)
		{
		}

		public abstract LoginToken Login ( object credential ) ;

		public void CheckToken(AccessToken token, LoginToken tokenToCheck)
		{
			HttpClient client = HttpClientFactory();

			client.DefaultRequestHeaders.Add(
											"token",
											JsonSerializer.Serialize(token));

			HttpResponseMessage response = client.PostAsJsonAsync(
																new UriBuilder(
																Uri.UriSchemeHttps,
																HostName,
																Port,
																$"{Type}/{nameof(CheckToken)}").Uri,
																tokenToCheck).
												Result;

			response.EnsureSuccessStatusCode();
		}

		public void DisposeToken(LoginToken token)
		{
			HttpClient client = HttpClientFactory();

			HttpResponseMessage response = client.PostAsJsonAsync(
																new UriBuilder(
																Uri.UriSchemeHttps,
																HostName,
																Port,
																$"{Type}/{nameof(DisposeToken)}").Uri,
																token).
												Result;

			response.EnsureSuccessStatusCode();
		}

		public override TimeSpan MeasureLatency()
		{
			DateTimeOffset firstTime  = GetTime();
			DateTimeOffset secondTime = GetTime();

			return secondTime - firstTime;
		}

	}

	public class RemoteLoginService <TCredential> : RemoteLoginService
		where TCredential : class
	{

		public RemoteLoginService ( (string HostName , int Port) server ) : base ( server )
		{
		}

		public override LoginToken Login(object credential) => Login(credential as TCredential);


		public LoginToken Login ( TCredential credential )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
													new UriBuilder (
													Uri . UriSchemeHttps ,
													HostName ,
													Port ,
													$"{Type}/{nameof ( Login )}" ) . Uri ,
													credential ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			LoginToken result = response . Content . ReadAsAsync <LoginToken> ( ) . Result ;

			return result ;
		}

	
	}

}
