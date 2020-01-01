﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System . Xml . Linq ;

using DreamRecorder . Directory . Logic . Entities ;
using DreamRecorder . Directory . Logic . Permissions ;

using DreamRecorder.ToolBox.General;

namespace DreamRecorder . Directory . Logic
{

	public class EntityAttribute : IEquatable<EntityAttribute>,ISelfSerializable
	{

		public bool Equals(EntityAttribute other)
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

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is EntityAttribute other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}

		public XElement ToXElement ( )
		{
			XElement result = new XElement(nameof(EntityAttribute));

			result.SetAttributeValue(nameof(Name), Name);
			result.SetAttributeValue(nameof(Owner),  Owner.Guid);
			result.SetAttributeValue(nameof(Value),    Value);
			result.SetAttributeValue(nameof(Guid),    Guid);

			return result;
		}

		public static bool operator ==(EntityAttribute left, EntityAttribute right) { return Equals(left, right); }

		public static bool operator !=(EntityAttribute left, EntityAttribute right) { return !Equals(left, right); }

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

			return Permissions.Access(target);
		}




	}

}