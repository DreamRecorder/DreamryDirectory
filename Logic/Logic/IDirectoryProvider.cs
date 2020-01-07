using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Tokens ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Logic
{

	public interface IDirectoryProvider
	{

		EntityToken Login([CanBeNull]LoginToken token);

		EntityToken ChangeToken ([NotNull] EntityToken token , Guid target ) ;

		void DisposeToken ([NotNull] EntityToken token ) ;


		AccessToken Access([NotNull] EntityToken token, Guid target);

		string GetProperty([NotNull] EntityToken token, Guid target, string name);

		void SetProperty([NotNull] EntityToken token, Guid target, string name, string value);

		/// <summary>
		/// Get Access of Property
		/// </summary>
		/// <param name="token"></param>
		/// <param name="target"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		AccessType AccessProperty([NotNull] EntityToken token, Guid target, string name);

		/// <summary>
		/// Check if an entity is a member of a group
		/// </summary>
		/// <param name="token"></param>
		/// <param name="group"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool Contain([NotNull] EntityToken token, Guid group, Guid entity);

		ICollection <Guid> ListGroup ([NotNull] EntityToken token , Guid group ) ;

		/// <summary>
		/// Add a entity to a group
		/// </summary>
		/// <param name="token"></param>
		/// <param name="group"></param>
		/// <param name="target"></param>
		void AddToGroup([NotNull] EntityToken token, Guid group, Guid target);

		void RemoveFromGroup([NotNull] EntityToken token, Guid group, Guid target);

		/// <summary>
		/// Allow other entity to check if this token is valid
		/// </summary>
		/// <param name="token">Entity's Entity Token</param>
		/// <param name="tokenToCheck">the token to check</param>
		/// <returns></returns>
		bool CheckToken ([NotNull] EntityToken token , AccessToken tokenToCheck ) ;

		/// <summary>
		/// Allow other Directory Service to check if this token is valid
		/// </summary>
		/// <param name="token">Directory‘s Entity Token</param>
		/// <param name="tokenToCheck">the token to check</param>
		/// <returns></returns>
		bool CheckToken ([NotNull] EntityToken token , [NotNull] EntityToken tokenToCheck ) ;

		Guid CreateUser ([NotNull] EntityToken token ) ;

		Guid CreateGroup ([NotNull] EntityToken token ) ;

		void RegisterLogin ([NotNull] EntityToken loginServiceToken , [NotNull] LoginToken targetToken ) ;

	}

}