using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace Logic
{

	public interface IDirectoryProvider
	{

		EntityToken Login(LoginToken token);

		AccessToken Access(EntityToken token, Guid target);

		string GetProperty(EntityToken token, Guid target, string name);

		void SetProperty(EntityToken token, Guid target, string name, string value);

		AccessType AccessProperty(EntityToken token, Guid target, string name);

		bool Contain(EntityToken token, Guid group, Guid user);

		ICollection <Guid> ListGroup ( EntityToken token , Guid group ) ;

		void AddToGroup(EntityToken token, Guid group, Guid target);

		void RemoveFromGroup(EntityToken token, Guid group, Guid target);

		bool CheckToken ( EntityToken token , AccessToken tokenToCheck ) ;

		


	}

}