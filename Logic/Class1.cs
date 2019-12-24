using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{

	public class UserToken
	{



	}

	public class TokenProvider
	{



	}

    public class User : Entity
    {

    }

    public class EntityAttribute : IEquatable<EntityAttribute>
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

        public static bool operator ==(EntityAttribute left, EntityAttribute right) { return Equals(left, right); }

        public static bool operator !=(EntityAttribute left, EntityAttribute right) { return !Equals(left, right); }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public PermissionGroup ReadPermissions { get; set; }

        public Entity Owner { get; set; }

        public string Value { get; set; }

        public AccessType Access(Entity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return ReadPermissions.Access(target);
        }

    }

    public class Entity : IEquatable<Entity>
    {

        public Guid Guid { get; set; }

        public HashSet<EntityAttribute> Attributes { get; set; }

        public virtual bool Contain(Entity entity, HashSet<Entity> checkedEntities = null)
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

        public override int GetHashCode() { return Guid.GetHashCode(); }

        public static bool operator ==(Entity left, Entity right) { return Equals(left, right); }

        public static bool operator !=(Entity left, Entity right) { return !Equals(left, right); }

    }


    public class Group : Entity
    {

        public HashSet<Entity> Members { get; set; }

        public override bool Contain(Entity entity, HashSet<Entity> checkedEntities = null)
        {
            checkedEntities = checkedEntities ?? new HashSet<Entity>();

            if (checkedEntities.Contains(this))
            {
                return base.Contain(entity, checkedEntities);
            }

            else
            {
                if (base.Contain(entity, checkedEntities))
                {
                    return true;
                }
                else
                {
                    foreach (Entity member in Members.Except(checkedEntities))
                    {
                        if (member.Contain(entity, checkedEntities))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

    }

    public enum PermissionStatus
    {

        Allow,

        Deny

    }

    public enum PermissionType
    {
        Read,
        Write,
    }

    public enum AccessType
    {

        ReadWrite,
        Read,
        None,

    }

    public class Permission
    {

        public PermissionStatus Status { get; set; }

        public PermissionType Type { get; set; }

        public Entity Target { get; set; }

    }

    public class PermissionGroup
    {
        public HashSet<Permission> Permissions { get; set; }

        public AccessType Access(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            bool? write = null;
            bool? read = null;

            List<Permission> affectedPermission = Permissions.Where((perm) => perm.Target.Contain(entity)).ToList();

            foreach (Permission permission in affectedPermission)
            {
                switch (permission.Status)
                {
                    case PermissionStatus.Allow:
                        {
                            switch (permission.Type)
                            {
                                case PermissionType.Read:
                                    {
                                        if (read != false)
                                        {
                                            read = true;
                                        }
                                        break;
                                    }
                                case PermissionType.Write:
                                    {
                                        if (write != false)
                                        {
                                            write = true;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case PermissionStatus.Deny:
                        {
                            switch (permission.Type)
                            {
                                case PermissionType.Read:
                                    {

                                        read = false;
                                        break;
                                    }
                                case PermissionType.Write:
                                    {
                                        write = false;
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }

            bool writeResult = write ?? false;
            bool readResult = read ?? false;

            if (readResult)
            {
                if (writeResult)
                {
                    return AccessType.ReadWrite;
                }
                else
                {
                    return AccessType.Read;
                }
            }
            else
            {
                return AccessType.None;
            }

        }
    }

    public class Class1
    {

    }

}
