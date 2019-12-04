using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{

    public class User : Entity
    {

    }

    public class EntityAttribute : IEquatable <EntityAttribute>
	{

		public bool Equals ( EntityAttribute other )
		{
			if ( ReferenceEquals ( null , other ) )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Guid . Equals ( other . Guid ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , obj ) )
			{
				return true ;
			}

			if ( obj . GetType ( ) != this . GetType ( ) )
			{
				return false ;
			}

			return Equals ( ( EntityAttribute ) obj ) ;
		}

		public override int GetHashCode ( )
		{
			return Guid . GetHashCode ( ) ;
		}

		public static bool operator == ( EntityAttribute left , EntityAttribute right ) { return Equals ( left , right ) ; }

		public static bool operator != ( EntityAttribute left , EntityAttribute right ) { return ! Equals ( left , right ) ; }

		public Guid Guid { get ; set ; }

        public string Name { get; set; }

        public PermissionGroup Permissions { get; set; }

        public Entity Owner { get; set; }

        public string Value { get; set; }

		public bool CanAccess ( Entity target )
		{
			if ( target == null )
			{
				throw new ArgumentNullException ( nameof ( target ) ) ;
			}

			return Permissions . IsAllowedToAccess ( target ) ;
		}

    }

    public class Entity : IEquatable<Entity>
    {

        public Guid Guid { get; set; }

        public HashSet<EntityAttribute> Attributes { get; set; }

        public bool Contain(Entity entity) { return IsMember(entity, new HashSet<Entity>()); }

        protected virtual bool IsMember(Entity entity, HashSet<Entity> checkedEntities)
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

            return obj.GetType() == GetType() && Equals((Entity)obj) ;
		}

        public override int GetHashCode() { return Guid.GetHashCode(); }

        public static bool operator ==(Entity left, Entity right) { return Equals(left, right); }

        public static bool operator !=(Entity left, Entity right) { return !Equals(left, right); }

    }


    public class Group : Entity
    {

        public HashSet<Entity> Members { get; set; }

        protected override bool IsMember(Entity entity, HashSet<Entity> checkedEntities)
        {
            if (base.IsMember(entity, checkedEntities))
            {
                return true;
            }
            else
			{
				foreach ( Entity member in Members.Except(checkedEntities) )
				{
					member . IsMember ( entity , checkedEntities ) ;
				}
              
                               
            }

            return false;
        }

    }

    public enum PermissionStatus
    {

        Allow,

        Deny

    }

    public class Permission
    {

        public PermissionStatus Status { get; set; }

        public Entity Target { get; set; }

    }

    public class PermissionGroup
    {
        public HashSet<Permission> Permissions { get; set; }

        public bool IsAllowedToAccess(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            bool result = false;

			List <Permission> affectedPermission= Permissions . Where ( ( perm ) => perm . Target . Contain ( entity ) ) .ToList();

            foreach (Permission permission in affectedPermission)
            {
                    switch (permission.Status)
                    {
                        case PermissionStatus.Allow:
                            {
                                result = true;
                                break;
                            }
                        case PermissionStatus.Deny:
                            {
                                return false;
                            }
                    }
            }
            return result;
        }
    }

    public class Class1
    {

    }

}
