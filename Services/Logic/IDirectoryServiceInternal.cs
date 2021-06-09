using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;
using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class NewUserEventArgs : EventArgs
	{

		public User User { get ; set ; }

	}

	public class NewLoginEventArgs : EventArgs
	{

		public Entity Entity { get; set ; }

	}

	public class UpdatePropertyEventArgs : EventArgs
	{

		public Entity Entity{ get; set; }

		public EntityProperty Property { get ;set; }

	}

	public static class KnownEvents
	{

		public static event EventHandler <NewUserEventArgs> NewUser ;

		public static event EventHandler <NewLoginEventArgs> NewLogin ;

	}

	public interface IDirectoryServiceInternal
	{

		DirectoryService ServiceEntity { get ; }

		ITokenPolicy TokenPolicy { get ; }

		IDirectoryDatabase DirectoryDatabase { get ; set ; }

		

		AccessToken IssueAccessToken ( [NotNull] Entity entity , [NotNull] Entity accessTarget , TimeSpan lifetime ) ;

		/// <summary>
		///     Issue entity token
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lifetime"></param>
		/// <returns></returns>
		EntityToken IssueEntityToken ( [NotNull] Entity entity , TimeSpan lifetime ) ;

	}

}
