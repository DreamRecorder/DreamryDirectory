using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . EntityFrameworkCore . ChangeTracking ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Group : Entity
	{

		public ObservableHashSet <Guid> Members { get ; } = new ObservableHashSet <Guid> ( ) ;

		public override bool Contain ( Entity entity , HashSet <Guid> checkedEntities = null )
		{
			checkedEntities ??= new HashSet <Guid> ( ) ;

			checkedEntities . Add ( Guid ) ;

			if ( checkedEntities . Contains ( Guid ) )
			{
				return base . Contain ( entity , checkedEntities ) ;
			}
			else
			{
				return base . Contain ( entity , checkedEntities )
					|| Members . Except ( checkedEntities ) .
								Any (
									member
										=> DirectoryServiceInternal . FindEntity ( member ) .
											Contain ( entity , checkedEntities ) ) ;
			}
		}

	}

}
