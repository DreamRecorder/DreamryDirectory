using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic ;

public class NewUserEventArgs : EventArgs
{

	public User User { get ; set ; }

}
