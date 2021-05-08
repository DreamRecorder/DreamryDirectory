using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public interface IDirectoryDatabaseStorage
	{

		HashSet <DbUser> GetDbUsers ( ) ;

		HashSet <DbDirectoryService> GetDbDirectoryServices ( ) ;

		HashSet <DbLoginService> GetDbLoginServices ( ) ;

		HashSet <DbGroup> GetDbGroups ( ) ;

		HashSet <DbGroupMember> GetDbGroupMembers ( ) ;

		HashSet <DbProperty> GetDbProperties ( ) ;

		void Save ( ) ;

		void DeleteGroupMember ( DbGroupMember groupMember ) ;

		void DeleteProperty ( DbProperty property ) ;

		HashSet <DbService> GetDbServices ( ) ;

	}

	public class DbService
	{
		public Guid Guid { get; set; }

		public HashSet<DbProperty> Proprieties { get; set; }
	}


}
