using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic ;

[AttributeUsage( AttributeTargets.Field)]
public class PropertyAttribute : Attribute
{
	public EntityScope Scope { get;  }

	public string Namespace { get; }

	public bool IsRequired { get; }

	public PropertyAttribute ( EntityScope scope , string ns , bool isRequired )
	{
		Scope      = scope ;
		Namespace  = ns ;
		IsRequired = isRequired ;
	}

}