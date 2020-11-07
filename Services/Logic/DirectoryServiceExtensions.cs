using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class DirectoryServiceExtensions
	{

		public static readonly string ApiEndPoints = nameof(ApiEndPoints);

		public static readonly string ApiEndPointsName = $"{Consts.Namespace}.{ApiEndPoints}";

		public static EntityProperty GetApiEndPointsProperty([NotNull] this DirectoryService directoryService)
		{
			if (directoryService == null)
			{
				throw new ArgumentNullException(nameof(directoryService));
			}

			if (directoryService.Properties.FirstOrDefault((prop) => prop.Name == ApiEndPointsName) is EntityProperty property)
			{

			}
			else
			{
				property = new EntityProperty()
							{
								Name  = ApiEndPointsName,
								Owner = DirectoryServiceInternal.Current.KnownSpecialGroups.DirectoryServices,
								Value = string.Empty
							};

				directoryService.Properties.Add(property);
			}

			return property;

		}

	}

}