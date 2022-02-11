using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System . Net . Http ;

namespace DreamRecorder . Directory . ServiceProvider ;

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