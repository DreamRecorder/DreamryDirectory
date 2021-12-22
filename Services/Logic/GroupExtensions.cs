using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Services.Logic.Entities;

using JetBrains.Annotations;

namespace DreamRecorder.Directory.Services.Logic
{

	public static class GroupExtensions
	{
		
		public static string Members => nameof(Members);

		public static string MembersName => $"{Constants.Namespace}.{Members}";


		public static EntityProperty GetMembersProperty([NotNull] this Group group)
		{
			if (group == null)
			{
				throw new ArgumentNullException(nameof(group));
			}


			return group.GetOrCreateProperty(MembersName);
		}

	}

}
