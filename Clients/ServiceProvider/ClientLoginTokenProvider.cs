using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;

namespace DreamRecorder.Directory.ServiceProvider;

public class ClientLoginTokenProvider : ILoginTokenProvider
{

	public ClientLoginTokenProvider ( ILoginServiceProvider loginServiceProvider , ICollection <(Guid loginProvider , object credential)> credentials )
	{
		LoginServiceProvider = loginServiceProvider ;
		Credentials          = credentials ;
	}

	public ICollection<(Guid loginProvider, object credential)> Credentials { get; }

	private ILoginServiceProvider LoginServiceProvider { get; }

	public LoginToken GetToken()
	{
		foreach ((Guid loginProvider, object credential) credential in Credentials)
		{
			try
			{
				return LoginServiceProvider.GetLoginService(credential.loginProvider).Login(credential.credential);
			}
			catch (Exception e)
			{
				continue;
			}
		}
		return null;
	}

}
