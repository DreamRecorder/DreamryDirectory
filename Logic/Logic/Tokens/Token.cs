﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System . Xml . Linq ;

using DreamRecorder.ToolBox.General;

namespace DreamRecorder . Directory . Logic . Tokens
{

	public abstract class Token: IEquatable <Token>
	{

		public Guid Owner { get; set; }

		public byte[] Secret { get; set; }

		public DateTimeOffset NotBefore { get; set; }

		public DateTimeOffset NotAfter { get; set; }

		public Guid Issuer { get; set; }

		public bool Equals ( Token other )
		{
			if ( other is null)
			{
				return false ;
			}

			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Owner . Equals ( other . Owner ) && Equals ( Secret , other . Secret ) && NotBefore . Equals ( other . NotBefore ) && NotAfter . Equals ( other . NotAfter ) && Issuer . Equals ( other . Issuer ) ;
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

			return Equals ( ( Token ) obj ) ;
		}

		public override int GetHashCode ( ) => HashCode . Combine ( Owner , Secret , NotBefore , NotAfter , Issuer ) ;

		public static bool operator == ( Token left , Token right ) => Equals ( left , right ) ;

		public static bool operator != ( Token left , Token right ) => ! Equals ( left , right ) ;

		public virtual XElement ToXElement ( ) { return ToXElement ( ) ; }

	}

}