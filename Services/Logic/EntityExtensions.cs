using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services .Logic
{

	public static class EntityExtensions
	{

		public static string Namespace => "DreamRecorder.Directory";

		public static readonly string IsDisabled = nameof ( IsDisabled ) ;

		public static readonly string IsDisabledName = $"{Namespace}.{IsDisabled}" ;

		public static bool GetIsDisabled ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if (entity.Attributes.FirstOrDefault(prop => prop.Name == IsDisabledName) is EntityAttribute attribute)
			{
				if ( bool.TryParse(attribute.Value,out bool result) )
				{
					return result;
				}
			}

			return false;
		}

		public static char CanLoginFromSeparator => ',' ;


		public static readonly string CanLoginFrom = nameof ( CanLoginFrom ) ;
		public static readonly string CanLoginFromName = $"{Namespace}.{CanLoginFrom}";



		public static ICollection <Guid> GetCanLoginFrom ( [NotNull] this Entity entity )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( entity.Attributes.FirstOrDefault( (prop)=>prop.Name==CanLoginFromName)is EntityAttribute attribute)
			{
				return attribute . Value . Split ( ',' ) .
							Select ( Guid . Parse ) .
							ToHashSet ( ) ; ;
			}

		}



	}

}