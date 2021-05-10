using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . Directory . Logic
{

	public readonly struct Permission : IEquatable <Permission>
	{

		public bool Equals ( Permission other )
			=> Status == other . Status && Type == other . Type && Target . Equals ( other . Target ) ;

		public override bool Equals ( object obj ) => obj is Permission other && Equals ( other ) ;

		public override int GetHashCode ( ) => HashCode . Combine ( ( int ) Status , ( int ) Type , Target ) ;

		public static bool operator == ( Permission left , Permission right ) => left . Equals ( right ) ;

		public static bool operator != ( Permission left , Permission right ) => ! left . Equals ( right ) ;

		public PermissionStatus Status { get ; }

		public PermissionType Type { get ; }

		public Guid Target { get ; }

		public Permission ( PermissionStatus status , PermissionType type , Guid target )
		{
			Status = status ;
			Type   = type ;
			Target = target ;
		}

	}

}
