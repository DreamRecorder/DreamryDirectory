using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Net ;
using System.Net.Http ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder.Directory.Logic.Tokens ;

namespace DreamRecorder . Directory . ServiceProvider
{

	public class RemoteLoginProvider <TCredential> : ILoginProvider where TCredential:class
	{

		public IPEndPoint ServiceEndPoint { get; set; }

		public LoginToken Login ( TCredential credential )
		{

			HttpClient client = new HttpClient ( ) ;

			//client . PostAsync (
			//					$"https://{ServiceEndPoint . Address}:{ServiceEndPoint . Port}/Login" , ) ;

			throw new NotImplementedException();
		}

		public LoginToken Login ( object credential ) => Login(credential as TCredential) ;

		public bool CheckToken ( AccessToken token , LoginToken tokenToCheck ) => throw new NotImplementedException ( ) ;


	}

}