using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public interface IDirectoryServiceInternal
	{
		DirectoryService ServiceEntity { get; }

		ITokenPolicy TokenPolicy { get; }

		AccessToken IssueAccessToken([NotNull] Entity entity, [NotNull] Entity accessTarget, TimeSpan lifetime);

		/// <summary>
		/// Issue entity token
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lifetime"></param>
		/// <returns></returns>
		EntityToken IssueEntityToken([NotNull] Entity entity, TimeSpan lifetime);

		IDirectoryDatabase DirectoryDatabase{ get; set; }
	}

}
