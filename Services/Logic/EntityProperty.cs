using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Services.Logic.Entities;
using DreamRecorder.ToolBox.General;

using JetBrains.Annotations;

using PermissionGroup = DreamRecorder.Directory.Services.Logic.Permissions.PermissionGroup;
using static DreamRecorder . Directory . Services . Logic . DirectoryServiceInternal;

namespace DreamRecorder.Directory.Services.Logic
{

	public static class EntityPropertyExtensions
	{

		public static AccessType Access(this EntityProperty property, Entity target)
		{
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			if (FindEntity(property.Owner).Contain(target ))
			{
				return AccessType.ReadWrite;
			}

			return property.Permissions.Access(target);
		}


	}

	public class EntityProperty : IEquatable<EntityProperty>, INotifyPropertyChanged
	{

		public string Name { get; set; }

		public PermissionGroup Permissions { get; set; } = new PermissionGroup();

		public Guid Owner { get; set; }

		public Guid Target { get; set; }

		public string Value { get; set; }

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

			return other.Name == Name && other.Owner == Owner;
		}


		public override bool Equals(object obj)
			=> ReferenceEquals(this, obj) || obj is EntityProperty other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Owner, Name);

		public static bool operator ==(EntityProperty left, EntityProperty right) => Equals(left, right);

		public static bool operator !=(EntityProperty left, EntityProperty right) => !Equals(left, right);


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}

}
