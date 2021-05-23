using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Services.Logic.Entities;

using JetBrains.Annotations;

using KnownPermissionGroups = DreamRecorder.Directory.Services.Logic.Entities.KnownPermissionGroups;
using Permission = DreamRecorder.Directory.Services.Logic.Permissions.Permission;
using PermissionGroup = DreamRecorder.Directory.Services.Logic.Permissions.PermissionGroup;


namespace DreamRecorder.Directory.Services.Logic
{

	public static class EntityExtensions
	{

		public static char CanLoginFromSeparator => ',';

		public static string DisplayName => nameof(DisplayName);

		public static string DisplayNameName => $"{Constants.Namespace}.{DisplayName}";

		public static string IsDisabled => nameof(IsDisabled);

		public static string IsDisabledName => $"{Constants.Namespace}.{IsDisabled}";

		public static string CanLoginFrom => nameof(CanLoginFrom);

		public static string CanLoginFromName => $"{Constants.Namespace}.{CanLoginFrom}";

		public static string StopRenewEntityToken => nameof(StopRenewEntityToken);

		public static string StopRenewEntityTokenName => $"{Constants.Namespace}.{StopRenewEntityToken}";

		public static EntityProperty GetOrCreateProperty(
			[NotNull] this Entity entity,
			[NotNull] string name,
				Entity owner = default,
				PermissionGroup permissionGroup = default,
			string defaultValue = default)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			owner ??= DirectoryServiceInternal.Current.DirectoryDatabase.KnownSpecialGroups.DirectoryServices;

			permissionGroup ??= KnownPermissionGroups.InternalApiOnly;

			if (permissionGroup == null)
			{
				throw new ArgumentNullException(nameof(permissionGroup));
			}

			if (entity.Properties.FirstOrDefault(prop => prop.Name == name) is not EntityProperty property)
			{
				property = new EntityProperty
				{
					Name = name,
					Owner = owner,
					Permissions = permissionGroup,
					Value = defaultValue,
				};

				entity.Properties.Add(property);
			}

			return property;

		}

		public static EntityProperty GetDisplayNameProperty([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			switch (entity)
			{
				case Group group:
					{
						return entity.GetOrCreateProperty(DisplayNameName, group.GetMembersProperty().Owner, KnownPermissionGroups.AuthorizedReadonly);

					}
				case User user:
				case Service service:
					{
						return entity.GetOrCreateProperty(DisplayNameName, entity, KnownPermissionGroups.AuthorizedReadonly);

					}
				case LoginService:
				case DirectoryService:
					{
						return entity.GetOrCreateProperty(DisplayNameName, entity, KnownPermissionGroups.EveryoneReadonly);
					}
				default:
					{
						return entity.GetOrCreateProperty(DisplayNameName, permissionGroup: KnownPermissionGroups.AuthorizedReadonly);

					}

			}


		}

		[CanBeNull]
		public static string GetDisplayName([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			return entity.GetDisplayNameProperty().Value;
		}

		public static EntityProperty GetIsDisabledProperty([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			return entity.GetOrCreateProperty(IsDisabledName, permissionGroup: KnownPermissionGroups.AuthorizedReadonly);
		}


		public static bool GetIsDisabled([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			return Convert.ToBoolean(entity.GetIsDisabledProperty().Value);

		}

		public static bool SetIsDisabled([NotNull] this Entity entity, bool isDisabled)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			entity.GetIsDisabledProperty().Value = isDisabled.ToString();
			return isDisabled;
		}

		public static EntityProperty GetCanLoginFromProperty([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			return entity.GetOrCreateProperty(CanLoginFromName, permissionGroup: KnownPermissionGroups.LoginServicesReadonly);
		}

		public static ICollection<Guid> GetCanLoginFrom([NotNull] this Entity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			return entity.GetCanLoginFromProperty().Value.Split(CanLoginFromSeparator, StringSplitOptions.RemoveEmptyEntries).
						Select(Guid.Parse).
						ToHashSet();
		}

		public static ICollection<Guid> AddCanLoginFrom([NotNull] this Entity entity, LoginService loginService)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			ICollection<Guid> canLoginFrom = GetCanLoginFrom(entity);

			canLoginFrom.Add(loginService.Guid);

			entity.GetCanLoginFromProperty().Value = string.Concat(
																	canLoginFrom.Select(
																	guid => guid.ToString()
																			+ CanLoginFromSeparator));

			return canLoginFrom;
		}

	}

}
