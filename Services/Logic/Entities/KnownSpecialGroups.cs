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

		public SpecialGroups SpecialGroups { get ; set ; }

		public DirectoryServices DirectoryServices { get; set; }

		public LoginServices LoginServices { get; set; }

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