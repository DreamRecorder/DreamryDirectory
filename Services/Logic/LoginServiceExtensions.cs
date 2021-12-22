using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Services.Logic.Entities;

using JetBrains.Annotations;

namespace DreamRecorder.Directory.Services.Logic
{

	public static class LoginServiceExtensions
	{

		public static readonly string DatabaseConnectionString = nameof(DatabaseConnectionString);

		public static readonly string DatabaseConnectionStringName =
			$"{Constants.Namespace}.{DatabaseConnectionString}";


		public static readonly string ApiEndPoints = nameof(ApiEndPoints);

		public static readonly string ApiEndPointsName = $"{Constants.Namespace}.{ApiEndPoints}";

		public static EntityProperty GetApiEndPointsProperty([NotNull] this LoginService loginService)
		{
			if (loginService == null)
			{
				throw new ArgumentNullException(nameof(loginService));
			}

			return loginService.GetOrCreateProperty(
													DatabaseConnectionStringName,
													DirectoryServiceInternal.Current.DirectoryDatabase.KnownSpecialGroups.DirectoryServices.Guid,
													KnownPermissionGroups.InternalApiOnly, default);

		}

		public static List<(string HostName, int Port)> GetApiEndPoints([NotNull] this LoginService loginService)
		{
			if (loginService == null)
			{
				throw new ArgumentNullException(nameof(loginService));
			}

			List<(string HostName, int Port)> result = new List<(string HostName, int Port)>();

			List<string> endPointsStrings = loginService.GetApiEndPointsProperty().
															Value.
															Split(',', StringSplitOptions.RemoveEmptyEntries).
															Select(str => str.Trim()).
															ToList();


			foreach (string endPointsString in endPointsStrings)
			{
				string[] endPointsParts = endPointsString.Split(':', StringSplitOptions.RemoveEmptyEntries).
															Select(str => str.Trim()).
															ToArray();

				if (string.IsNullOrEmpty(endPointsParts.FirstOrDefault()))
				{
					string hostname = endPointsParts[0];

					if (endPointsParts.Length == 2)
					{
						result.Add((hostname, int.Parse(endPointsParts[1])));
					}
					else
					{
						result.Add((hostname, 443));
					}
				}
			}

			return result;
		}

	}

}
