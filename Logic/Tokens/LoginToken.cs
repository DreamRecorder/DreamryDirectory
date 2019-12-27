using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder . Directory . Logic . Entities ;

namespace DreamRecorder . Directory . Logic . Tokens
{

    public class LoginToken : Token
    {

        public Guid Issuer { get; set; }

        public string DisplayName { get; set; }

    }

}
