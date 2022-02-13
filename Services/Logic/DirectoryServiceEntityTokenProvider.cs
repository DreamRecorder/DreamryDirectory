using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . ServiceProvider ;

namespace DreamRecorder . Directory . Services . Logic ;

public class DirectoryServiceEntityTokenProvider : IEntityTokenProvider
{

	public DirectoryServiceEntityTokenProvider ( IDirectoryServiceInternal directoryServiceInternal )
	{
		DirectoryServiceInternal = directoryServiceInternal ;
	}

	public IDirectoryServiceInternal DirectoryServiceInternal { get ; }

	public Guid EntityGuid => DirectoryServiceInternal . ServiceEntity . Guid ;

	public EntityToken GetToken ( )
		=> DirectoryServiceInternal . IssueEntityToken (
														DirectoryServiceInternal . ServiceEntity ,
														DirectoryServiceInternal . TokenPolicy . EntityTokenLife (
														DirectoryServiceInternal . ServiceEntity ) ) ;

}