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

	public class RemoteDirectoryService : IDirectoryService
	{

		public Func <HttpClient> HttpClientFactory { get ; set ; } = ( ) => new HttpClient ( ) ;

		public string Server { get ; set ; }

		public int Port { get ; set ; }

		public DateTime GetTime ( ) => throw new NotImplementedException ( ) ;

		public Version GetVersion ( ) => throw new NotImplementedException ( ) ;

		public EntityToken Login ( LoginToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( Login ) ) . Uri ,
																	token ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			EntityToken result = response . Content . ReadAsAsync <EntityToken> ( ) . Result ;

			return result ;
		}

		public EntityToken UpdateToken ( EntityToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				nameof ( UpdateToken ) ) . Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			EntityToken result = response . Content . ReadAsAsync <EntityToken> ( ) . Result ;

			return result ;
		}

		public void DisposeToken ( EntityToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( DisposeToken ) ) . Uri ,
																	token ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public void DisposeToken ( LoginToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( DisposeToken ) ) . Uri ,
																	token ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public void DisposeToken ( AccessToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( DisposeToken ) ) . Uri ,
																	token ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public AccessToken Access ( EntityToken token , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( Access )}/{target}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			AccessToken result = response . Content . ReadAsAsync <AccessToken> ( ) . Result ;

			return result ;
		}

		public string GetProperty ( EntityToken token , Guid target , string name )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( GetProperty )}/{target}/{name}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			string result = response . Content . ReadAsAsync <string> ( ) . Result ;

			return result ;
		}

		public Guid GetPropertyOwner ( EntityToken token , Guid target , string name )
		{
			HttpClient client = HttpClientFactory();

			client.DefaultRequestHeaders.Add("token", JsonSerializer.Serialize(token));

			HttpResponseMessage response = client.PostAsync(
															new UriBuilder(
																			Uri.UriSchemeHttps,
																			Server,
																			Port,
																			$"{nameof(GetPropertyOwner)}/{target}/{name}").
																Uri,
															null).
												Result;

			response.EnsureSuccessStatusCode();

			Guid result = response.Content.ReadAsAsync<Guid>().Result;

			return result;
		}

		public void SetProperty ( EntityToken token , Guid target , string name , string value )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																		Uri . UriSchemeHttps ,
																		Server ,
																		Port ,
																		$"{nameof ( SetProperty )}/{target}/{name}" ) .
																		Uri ,
																	value ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public void TransferProperty ( EntityToken token , Guid target , string name , Guid newOwner )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( TransferProperty )}/{target}/{name}/{newOwner}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public AccessType AccessProperty ( EntityToken token , Guid target , string name )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( AccessProperty )}/{target}/{name}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			AccessType result = response . Content . ReadAsAsync <AccessType> ( ) . Result ;

			return result ;
		}

		public void SetPropertyPermission ( EntityToken token , Guid target , string name , Guid permissionGroup )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( SetPropertyPermission )}/{target}/{name}/{permissionGroup}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public PermissionGroup GetPermissionGroup ( EntityToken token , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . GetAsync (
															new UriBuilder (
																			Uri . UriSchemeHttps ,
																			Server ,
																			Port ,
																			$"{nameof ( GetPermissionGroup )}/{target}" ) .
																Uri ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			PermissionGroup result = response . Content . ReadAsAsync <PermissionGroup> ( ) . Result ;

			return result ;
		}

		public PermissionGroup UpdatePermissionGroup ( EntityToken token , PermissionGroup target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																		Uri . UriSchemeHttps ,
																		Server ,
																		Port ,
																		$"{nameof ( UpdatePermissionGroup )}/{target . Guid}" ) .
																		Uri ,
																	target ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			PermissionGroup result = response . Content . ReadAsAsync <PermissionGroup> ( ) . Result ;

			return result ;
		}

		public bool Contain ( EntityToken token , Guid group , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( Contain )}/{group}/{target}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			bool result = response . Content . ReadAsAsync <bool> ( ) . Result ;

			return result ;
		}

		public ICollection <Guid> ListGroup ( EntityToken token , Guid group )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( ListGroup )}/{group}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			ICollection <Guid> result = response . Content . ReadAsAsync <ICollection <Guid>> ( ) . Result ;

			return result ;
		}

		public void AddToGroup ( EntityToken token , Guid group , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( AddToGroup )}/{group}/{target}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public void RemoveFromGroup ( EntityToken token , Guid group , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( RemoveFromGroup )}/{group}/{target}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public void CheckToken ( EntityToken token , AccessToken tokenToCheck )
		{
			HttpClient client = HttpClientFactory ( ) ;

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

		public void CheckToken ( EntityToken token , EntityToken tokenToCheck )
		{
			HttpClient client = HttpClientFactory ( ) ;

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

		public Guid CreateUser ( EntityToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( CreateUser )}" ) . Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			Guid result = response . Content . ReadAsAsync <Guid> ( ) . Result ;

			return result ;
		}

		public Guid CreateGroup ( EntityToken token )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( CreateUser )}" ) . Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			Guid result = response . Content . ReadAsAsync <Guid> ( ) . Result ;

			return result ;
		}

		public void RegisterLogin ( EntityToken token , LoginToken targetToken )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsJsonAsync (
																	new UriBuilder (
																	Uri . UriSchemeHttps ,
																	Server ,
																	Port ,
																	nameof ( RegisterLogin ) ) . Uri ,
																	targetToken ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;
		}

		public TimeSpan GetLoginTokenLife ( EntityToken token , Guid target )
		{
			HttpClient client = HttpClientFactory ( ) ;

			client . DefaultRequestHeaders . Add ( "token" , JsonSerializer . Serialize ( token ) ) ;

			HttpResponseMessage response = client . PostAsync (
																new UriBuilder (
																				Uri . UriSchemeHttps ,
																				Server ,
																				Port ,
																				$"{nameof ( GetLoginTokenLife )}" ) .
																	Uri ,
																null ) .
													Result ;

			response . EnsureSuccessStatusCode ( ) ;

			TimeSpan result = response . Content . ReadAsAsync <TimeSpan> ( ) . Result ;

			return result ;
		}

	}

}
