﻿using System ;
using System . Collections . Generic ;
using System . Text ;

namespace DreamRecorder . Directory . Services . Logic . Storage
{

	public class DbUser : IEquatable <DbUser>
	{

		public Guid Guid { get ; set ; }

		public bool Equals ( DbUser other )
		{
			if ( other is null)
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Guid . Equals ( other . Guid ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null)
			{
				return false ;
			}

			if ( ReferenceEquals ( this , obj ) )
			{
				return true ;
			}

			if ( obj . GetType ( ) != GetType ( ) )
			{
				return false ;
			}

			return Equals ( ( DbUser ) obj ) ;
		}

		public override int GetHashCode ( ) { return Guid . GetHashCode ( ) ; }

		public static bool operator == ( DbUser left , DbUser right ) { return Equals ( left , right ) ; }

		public static bool operator != ( DbUser left , DbUser right ) { return ! Equals ( left , right ) ; }

		public HashSet <DbProperty> Proprieties { get ; set ; }

	}

}