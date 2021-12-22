using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Services.General;
using DreamRecorder.Directory.Services.Logic.Entities;

using JetBrains.Annotations;

using Microsoft . EntityFrameworkCore . ChangeTracking ;

namespace DreamRecorder.Directory.Services.Logic.Permissions
{

	public class PermissionGroup
	{

		public Guid Guid { get; set; }

		public Guid Owner { get; set; }

		public ObservableHashSet <Permission> Permissions { get ; } = new ObservableHashSet <Permission> ( ) ;

		public Directory.Logic.PermissionGroup ToClientSidePermissionGroup()
		{
			Directory.Logic.PermissionGroup result = new Directory.Logic.PermissionGroup
			{
				Guid = Guid,
				Owner = Owner,
				Permissions = Permissions.ToHashSet(),
			};

			return result;
		}

		public void Edit([NotNull] Directory.Logic.PermissionGroup permissionGroup)
		{
			if (permissionGroup == null)
			{
				throw new ArgumentNullException(nameof(permissionGroup));
			}

			Entity newOwner =
				DirectoryServiceInternal.Current.DirectoryDatabase.FindEntity(permissionGroup.Owner);

			if (newOwner == null)
			{
				throw new TargetEntityNotFoundException(permissionGroup.Owner);
			}

			Permissions . RemoveWhere ( ( permission ) => ! permissionGroup . Permissions . Contains ( permission ) ) ;

			Permissions.UnionWith ( permissionGroup.Permissions ) ;
		}

		public AccessType Access(Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			bool? write = null;
			bool? read = null;

			HashSet<Permission> affectedPermission =
				Permissions.Where(perm => DirectoryServiceInternal.Current.DirectoryDatabase.FindEntity(perm.Target).Contain(entity)).ToHashSet();

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

}
