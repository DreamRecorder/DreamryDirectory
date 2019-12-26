using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace Logic
{

	public interface ILoginProvider
	{

		LoginToken Login(object credential);

	}

}