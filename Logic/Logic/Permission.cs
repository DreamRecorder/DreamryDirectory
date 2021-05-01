using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic
{

	public readonly struct Permission : IEquatable<Permission>
	{

		public bool Equals(Permission other)
		{
			return Status == other.Status && Type == other.Type && Target.Equals(other.Target);
		}

		public override bool Equals(object obj)
		{
			return obj is Permission other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine((int)Status, (int)Type, Target);
		}

		public static bool operator ==(Permission left, Permission right) { return left.Equals(right); }

		public static bool operator !=(Permission left, Permission right) { return !left.Equals(right); }

		public PermissionStatus Status { get; }

		public PermissionType Type { get; }

		public Guid Target { get; }

		public Permission(PermissionStatus status, PermissionType type, Guid target)
		{
			Status = status;
			Type   = type;
			Target = target;
		}

	}

}