using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Logic
{

	public interface IDirectoryService
	{

		[CanBeNull]
		EntityToken Login ( [CanBeNull] LoginToken token ) ;

		[CanBeNull]
		EntityToken UpdateToken ( [NotNull] EntityToken token ) ;

		void DisposeToken ( [NotNull] EntityToken token ) ;

		void DisposeToken ( [NotNull] LoginToken token ) ;

		void DisposeToken ( [NotNull] AccessToken token ) ;

		[CanBeNull]
		AccessToken Access ( [NotNull] EntityToken token , Guid target ) ;

		[CanBeNull]
		string GetProperty ( [NotNull] EntityToken token , Guid target , string name ) ;

		void SetProperty ( [NotNull] EntityToken token , Guid target , string name , string value ) ;

		void TransferProperty( [NotNull] EntityToken token , Guid target , string name , Guid newOwner ) ;

		/// <summary>
		///     Get Access of Property
		/// </summary>
		/// <param name="token"></param>
		/// <param name="target"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		AccessType AccessProperty ( [NotNull] EntityToken token , Guid target , string name ) ;

		void SetPropertyPermission ( [NotNull] EntityToken token , Guid target , string name , Guid permissionGroup ) ;

		[CanBeNull] 
		PermissionGroup GetPermissionGroup ( [NotNull] EntityToken token , Guid target ) ;

		PermissionGroup UpdatePermissionGroup ( [NotNull] EntityToken token , PermissionGroup target ) ;

		/// <summary>
		///     Check if an entity is a member of a group
		/// </summary>
		/// <param name="token"></param>
		/// <param name="group"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		bool Contain ( [NotNull] EntityToken token , Guid group , Guid target ) ;

		[NotNull]
		ICollection <Guid> ListGroup ( [NotNull] EntityToken token , Guid group ) ;

		/// <summary>
		///     Add a entity to a group
		/// </summary>
		/// <param name="token"></param>
		/// <param name="group"></param>
		/// <param name="target"></param>
		void AddToGroup ( [NotNull] EntityToken token , Guid group , Guid target ) ;

		void RemoveFromGroup ( [NotNull] EntityToken token , Guid group , Guid target ) ;

		/// <summary>
		///     Allow other entity to check if this token is valid
		/// </summary>
		/// <param name="token">Entity's Entity Token</param>
		/// <param name="tokenToCheck">the token to check</param>
		/// <returns></returns>
		void CheckToken ( [NotNull] EntityToken token , AccessToken tokenToCheck ) ;

		/// <summary>
		///     Allow other Directory Service to check if this token is valid
		/// </summary>
		/// <param name="token">Directory‘s Entity Token</param>
		/// <param name="tokenToCheck">the token to check</param>
		/// <returns></returns>
		void CheckToken ( [NotNull] EntityToken token , [NotNull] EntityToken tokenToCheck ) ;

		Guid CreateUser ( [NotNull] EntityToken token ) ;

		Guid CreateGroup ( [NotNull] EntityToken token ) ;

		void RegisterLogin ( [NotNull] EntityToken token , [NotNull] LoginToken targetToken ) ;

		TimeSpan GetLoginTokenLife ( [NotNull] EntityToken token , Guid target ) ;

	}

}
