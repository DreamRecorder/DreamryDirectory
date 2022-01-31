using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . LoginProviders . ServiceProvider ;

public class LoginProvider : ILoginProvider
{
	private static ReadOnlyDictionary<Guid, LoginServiceBase> LoginServices { get; set; }

	[Prepare]
	public static void Prepare()
	{
		lock (StaticServiceProvider.ServiceCollection)
		{
			LoginServices = new ReadOnlyDictionary<Guid, LoginServiceBase>(AppDomainExtensions.FindType((type) => type.IsAssignableTo(typeof(LoginServiceBase))).Distinct().Select(Activator.CreateInstance).Cast<LoginServiceBase>().ToDictionary(loginService => loginService.Type));
		}
	}

	public ILoginService GetLoginService(Guid guid)
	{
		if (LoginServices.ContainsKey(guid))
		{
			return LoginServices[guid];
		}
		else
		{
			return null;
		}
	}

}
