using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Linq ;
using System . Runtime . CompilerServices ;

using DreamRecorder . Directory . Services . Logic . Permissions ;

using JetBrains . Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class EntityProperty : IEquatable <EntityProperty> , INotifyPropertyChanged
	{

		public string Name { get ; set ; }

		public PermissionGroup Permissions { get ; set ; } = new PermissionGroup ( ) ;

		public Guid Owner { get ; set ; }

		public Guid Target { get ; set ; }

		public string Value { get ; set ; }

		public bool Equals ( EntityProperty other )
		{
			if ( other is null )
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return other . Name == Name && other . Owner == Owner ;
		}


		public event PropertyChangedEventHandler PropertyChanged ;


		public override bool Equals ( object obj )
			=> ReferenceEquals ( this , obj ) || obj is EntityProperty other && Equals ( other ) ;

		public override int GetHashCode ( ) => HashCode . Combine ( Owner , Name ) ;

		public static bool operator == ( EntityProperty left , EntityProperty right )
			=> Equals ( left , right ) ;

		public static bool operator != ( EntityProperty left , EntityProperty right )
			=> ! Equals ( left , right ) ;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
		}

	}

}
