using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class EntityProperty : IEquatable <EntityProperty> , ISelfSerializable
	{

		public string Name { get ; set ; }

		public PermissionGroup Permissions { get ; set ; } = new PermissionGroup ( ) ;

		public Entity Owner { get ; set ; }

		public string Value { get ; set ; }

		public bool Equals ( EntityProperty other )
		{
			if ( other is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return other . Name == Name && other . Owner == Owner ;
		}

		public XElement ToXElement ( )
		{
			XElement result = new XElement ( nameof ( EntityProperty ) ) ;

			result . SetAttributeValue ( nameof ( Name ) ,  Name ) ;
			result . SetAttributeValue ( nameof ( Owner ) , Owner . Guid ) ;
			result . SetAttributeValue ( nameof ( Value ) , Value ) ;

			return result ;
		}

		public override bool Equals ( object obj )
		{
			return ReferenceEquals ( this , obj ) || obj is EntityProperty other && Equals ( other ) ;
		}

		public override int GetHashCode ( ) { return HashCode . Combine ( Owner , Name ) ; }

		public static bool operator == ( EntityProperty left , EntityProperty right )
		{
			return Equals ( left , right ) ;
		}

		public static bool operator != ( EntityProperty left , EntityProperty right )
		{
			return ! Equals ( left , right ) ;
		}

		public AccessType Access ( Entity target )
		{
			if ( target == null )
			{
				throw new ArgumentNullException ( nameof ( target ) ) ;
			}

			if ( target == Owner )
			{
				return AccessType . ReadWrite ;
			}

			return Permissions . Access ( target ) ;
		}

	}

}
