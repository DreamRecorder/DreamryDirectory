using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class KnownSpecialGroups
	{

		public Everyone Everyone { get ; set ; } = new Everyone ( ) ;

		public AuthorizedEntities AuthorizedEntities { get ; set ; } = new AuthorizedEntities ( ) ;

		public SpecialGroups SpecialGroups { get ; set ; } = new SpecialGroups ( ) ;

		public DirectoryServices DirectoryServices { get ; set ; } = new DirectoryServices ( ) ;

		public LoginServices LoginServices { get ; set ; } = new LoginServices ( ) ;

		public KnownSpecialGroups ( ) {

		}

		public List <SpecialGroup> Entities
			=> typeof ( KnownSpecialGroups ) . GetProperties ( ) .
												Where (
														prop
															=> typeof ( SpecialGroup ) . IsAssignableFrom (
															prop . PropertyType ) ).Select(prop=>prop.GetValue(this) as SpecialGroup) .
												ToList ( ) ;

	}

}