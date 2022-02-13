using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;
using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic ;

public class DirectoryServiceAccessTokenProvider : IAccessTokenProvider
{

	public DirectoryServiceAccessTokenProvider ( IDirectoryServiceInternal directoryServiceInternal )
	{
		DirectoryServiceInternal = directoryServiceInternal ;
	}

	public IDirectoryServiceInternal DirectoryServiceInternal { get ; }

	public Guid EntityGuid => DirectoryServiceInternal . ServiceEntity . Guid ;

	public AccessToken Access ( Guid target )
	{
		Entity targetEntity = DirectoryServiceInternal . DirectoryDatabase . FindEntity ( target ) ;
		return DirectoryServiceInternal . IssueAccessToken (
															DirectoryServiceInternal . ServiceEntity ,
															targetEntity ,
															DirectoryServiceInternal . TokenPolicy . AccessTokenLife (
															DirectoryServiceInternal . ServiceEntity ,
															targetEntity ) ) ;
	}

}
