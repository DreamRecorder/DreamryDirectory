﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using JetBrains.Annotations ;

namespace DreamRecorder . Directory . Services . Logic . Entities
{

	public class Entity : IEquatable<Entity>
	{

		public Guid Guid { get; set; }

		public HashSet<EntityProperty> Properties { get; set; }

		public virtual bool Contain([CanBeNull]Entity entity, HashSet<Entity> checkedEntities = null)
		{
			if (checkedEntities.Contains(this))
			{
				checkedEntities.Add(this);
			}

			return this == entity;
		}

		public bool Equals(Entity other)
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
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return obj.GetType() == GetType() && Equals((Entity)obj);
		}

		public override int GetHashCode() => Guid.GetHashCode() ;

		public static bool operator ==(Entity left, Entity right) => Equals(left, right) ;

		public static bool operator !=(Entity left, Entity right) => !Equals(left, right) ;

	}

}