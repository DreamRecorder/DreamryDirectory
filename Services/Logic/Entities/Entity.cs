using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . ComponentModel ;
using System . Linq ;
using System . Runtime . CompilerServices ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public abstract class Entity : IEquatable <Entity>
	{

		public virtual Guid Guid { get ; set ; }

		public bool Equals ( Entity other )
		{
			if ( other is null )
			{
				return false ;
			}

			return ReferenceEquals ( this , other ) || Guid . Equals ( other . Guid ) ;
		}

		public virtual bool Contain ( [NotNull] Entity entity , [CanBeNull] HashSet <Guid> checkedEntities = null )
		{
			if ( checkedEntities ? . Contains ( Guid ) == false )
			{
				checkedEntities . Add ( Guid ) ;
			}

			return this == entity ;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , obj ) )
			{
				return true ;
			}

			return obj . GetType ( ) == GetType ( ) && Equals ( ( Entity ) obj ) ;
		}

		public override int GetHashCode ( ) => Guid . GetHashCode ( ) ;

		public static bool operator == ( Entity left , Entity right ) => Equals ( left , right ) ;

		public static bool operator != ( Entity left , Entity right ) => ! Equals ( left , right ) ;
		
	}

}
