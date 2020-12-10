using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public static class GroupExtensions
	{

		public static readonly string Members = nameof ( Members ) ;

		public static readonly string MembersName = $"{Consts . Namespace}.{Members}" ;

		public static EntityProperty GetMembersProperty ( [NotNull]this Group group )
		{
			if ( @group == null )
			{
				throw new ArgumentNullException ( nameof ( @group ) ) ;
			}

			if ( group . Properties . FirstOrDefault ( ( prop ) => prop . Name == MembersName) is EntityProperty property )
			{

			}
			else
			{
				property = new EntityProperty ( )
							{
								Name  = MembersName ,
								Owner = DirectoryServiceInternal . Current .DirectoryDatabase.KnownSpecialGroups . DirectoryServices ,
								Value = string . Empty
							} ;

				group . Properties . Add ( property ) ;
			}

			return property;

		}

	}

	public static class SpecialClassExtensions
	{

		public static readonly string Members = nameof(Members);

		public static readonly string MembersName = $"{Consts.Namespace}.{Members}";

		public static EntityProperty GetMembersProperty([NotNull] this SpecialGroup specialGroup)
		{
			if (specialGroup == null)
			{
				throw new ArgumentNullException(nameof(specialGroup));
			}

			if (specialGroup.Properties.FirstOrDefault((prop) => prop.Name == MembersName) is EntityProperty property)
			{

			}
			else
			{
				property = new EntityProperty()
							{
								Name = MembersName,
								Owner = DirectoryServiceInternal.Current.DirectoryDatabase.KnownSpecialGroups.DirectoryServices,
								Value = string.Empty
							};

				specialGroup.Properties.Add(property);
			}

			return property;

		}

	}

}