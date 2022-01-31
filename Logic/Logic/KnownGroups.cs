using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DreamRecorder.Directory.Logic;

public static class KnownGroups
{

    public static Guid ServicingDirectoryServices
        => Guid.Parse("D5287823-3CF4-4047-942E-34DE9BFBDA19");

}

public static class KnownNamespaces
{

    public const string DirectoryServicesNamespace = "DreamRecorder.Directory";

}

[Flags]
public enum EntityScope:byte
{

    Anonymous = 1 << 0,

    User = 1 << 1,

    DirectoryService = 1 << 2,

    LoginService = 1 << 3,

    Group = 1 << 4,

    Service = 1 << 5,

    SpecialGroup = 1 << 6,

    Any=byte.MaxValue,

}

[AttributeUsage(AttributeTargets.Field)]
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

public static class KnownProperties
{

	public const string DisplayName = nameof(DisplayName);

	public const string IsDisabled = nameof(IsDisabled);


    [Property(EntityScope.DirectoryService,KnownNamespaces.DirectoryServicesNamespace,false)]
	public const string ApiEndpoints = nameof(ApiEndpoints);



}
