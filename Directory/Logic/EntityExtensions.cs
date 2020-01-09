using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder.Directory.Logic.Entities ;

namespace DreamRecorder . Directory .Logic
{

	public static class EntityExtensions
	{

		public static readonly string IsDisabled = nameof ( IsDisabled ) ;

		public static bool GetIsDisabled ( this Entity entity )
		{
			if (entity.Attributes.FirstOrDefault(prop => prop.Name == IsDisabled) is EntityAttribute attribute)
			{
				if ( bool.TryParse(attribute.Value,out bool result) )
				{
					return result;
				}
			}

			return false;
		}

	}

}