using System;
using System.Collections;
using System . Collections . Concurrent ;
using System.Collections.Generic;
using System . Collections . ObjectModel ;
using System.Linq;

using DreamRecorder.ToolBox.General;

namespace DreamRecorder.Directory.LoginProviders.ServiceProvider;

public class HashProvider : IHashProvider
{
    private static ReadOnlyDictionary<Guid, IHash> Hashs { get; set; }

    [Prepare]
    public static void Prepare()
    {
        lock (StaticServiceProvider.ServiceCollection)
        {
           Hashs=new ReadOnlyDictionary<Guid, IHash>( AppDomainExtensions.FindType((type) => type.IsAssignableTo(typeof(IHash))).Distinct().Select(Activator.CreateInstance).Cast<IHash>().ToDictionary(hash => hash.Guid));
        }
    }

	public IHash GetHash ( Guid guid )
	{
		if ( Hashs.ContainsKey(guid) )
		{
			return Hashs[guid];
        }
		else
		{
            return null;
		}
    }

}
