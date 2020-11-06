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

		public static readonly string ApiHost = nameof(ApiHost);

		public static readonly string ApiHostName = $"{Consts.Namespace}.{ApiHost}";

		public static EntityProperty GetMembersProperty([NotNull] this DirectoryService group)
		{
			if (@group == null)
			{
				throw new ArgumentNullException(nameof(@group));
			}

			if (group.Properties.FirstOrDefault((prop) => prop.Name == ApiHostName) is EntityProperty property)
			{

			}
			else
			{
				property = new EntityProperty()
							{
								Name  = ApiHostName,
								Owner = DirectoryServiceInternal.Current.KnownSpecialGroups.DirectoryServices,
								Value = string.Empty
							};

				group.Properties.Add(property);
			}

			return property;

		}

	}

}