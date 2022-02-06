using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using DreamRecorder.Directory.Logic.Tokens;

namespace DreamRecorder.Directory.ServiceProvider
{

    public interface IEntityTokenProvider
    {

        Guid EntityGuid { get; }

        EntityToken GetToken();

    }

}
