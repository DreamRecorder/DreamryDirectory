using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;

namespace DreamRecorder.Directory.LoginProviders.PreSharedKeyLoginProvider.ServiceProvider
{

	public class PreSharedKeyCredential
	{

		public Guid Target { get ; set ; }
		
		public string PreSharedKey{ get; set; }

	}

	public abstract class LoginProviderBase<TCredential> : ILoginProvider
	{

		public abstract Guid ? CheckCredential ( TCredential credential ) ;
		

		public LoginToken Login(object credential)
		{
			if (credential is TCredential tCredential)
			{
				if ( expr )
				{
					

				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		public void CheckToken(AccessToken token, LoginToken tokenToCheck) {; }


	}

	public class PreSharedKeyLoginProvider : ILoginProvider
	{

		public LoginToken Login ( object credential )
		{
			if ( credential is PreSharedKeyCredential preSharedKeyCredential )
			{
				
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		public void CheckToken(AccessToken token, LoginToken tokenToCheck) { ; }
	}
}
