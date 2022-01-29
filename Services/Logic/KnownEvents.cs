using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Services . Logic ;

public static class KnownEvents
{

	public static event EventHandler <NewUserEventArgs> NewUser ;

	public static event EventHandler <NewLoginEventArgs> NewLogin ;

}
