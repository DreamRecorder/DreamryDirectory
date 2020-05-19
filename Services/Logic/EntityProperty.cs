using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Xml.Linq ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder . Directory . Services . Logic . Entities ;
using DreamRecorder . Directory . Services . Logic . Permissions ;
using DreamRecorder.ToolBox.General ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class EntityProperty : IEquatable<EntityProperty>,ISelfSerializable
	{

		public bool Equals(EntityProperty other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Guid.Equals(other.Guid);
		}

		public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is EntityProperty other && Equals(other) ;

		public override int GetHashCode() => Guid.GetHashCode() ;

		public XElement ToXElement ( )
		{
			XElement result = new XElement(nameof(EntityProperty));

			result.SetAttributeValue(nameof(Name), Name);
			result.SetAttributeValue(nameof(Owner),  Owner.Guid);
			result.SetAttributeValue(nameof(Value),    Value);
			result.SetAttributeValue(nameof(Guid),    Guid);

			return result;
		}

		public static bool operator ==(EntityProperty left, EntityProperty right) => Equals(left, right) ;

		public static bool operator !=(EntityProperty left, EntityProperty right) => !Equals(left, right) ;

		public Guid Guid { get; set; }

		public string Name { get; set; }

		public PermissionGroup Permissions { get; set; }

		public Entity Owner { get; set; }

		public string Value { get; set; }

		public AccessType Access(Entity target)
		{
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			if ( target==Owner )
			{
				return AccessType . ReadWrite ;
			}

			return Permissions.Access(target);
		}




	}

}