using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic ;

public static class EntityPropertyExtensions
{

	public static AccessType Access ( this EntityProperty property , Entity target )
	{
		if ( target == null )
		{
			throw new ArgumentNullException ( nameof ( target ) ) ;
		}

		if ( DirectoryServiceInternal . FindEntity ( property . Owner ) . Contain ( target ) )
		{
			return AccessType . ReadWrite ;
		}

		return property . Permissions . Access ( target ) ;
	}

}
