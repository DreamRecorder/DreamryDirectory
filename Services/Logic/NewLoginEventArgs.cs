using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . Directory . Services . Logic . Entities ;

namespace DreamRecorder . Directory . Services . Logic ;

public class NewLoginEventArgs : EventArgs
{

	public Entity Entity { get ; set ; }

}
