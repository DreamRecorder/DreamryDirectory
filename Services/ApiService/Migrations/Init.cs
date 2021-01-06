using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . EntityFrameworkCore . Migrations ;

namespace DreamRecorder . Directory . Services . ApiService . Migrations
{

	public partial class Init : Migration
	{

		protected override void Up ( MigrationBuilder migrationBuilder )
		{
			migrationBuilder . CreateTable (
											"DbDirectoryServices" ,
											table => new { Guid = table . Column <Guid> ( nullable : false ) } ,
											constraints : table =>
														{
															table . PrimaryKey (
																				"PK_DbDirectoryServices" ,
																				x => x . Guid ) ;
														} ) ;

			migrationBuilder . CreateTable (
											"DbGroups" ,
											table => new { Guid = table . Column <Guid> ( nullable : false ) } ,
											constraints : table =>
														{
															table . PrimaryKey ( "PK_DbGroups" , x => x . Guid ) ;
														} ) ;

			migrationBuilder . CreateTable (
											"DbLoginServices" ,
											table => new { Guid = table . Column <Guid> ( nullable : false ) } ,
											constraints : table =>
														{
															table . PrimaryKey (
																				"PK_DbLoginServices" ,
																				x => x . Guid ) ;
														} ) ;

			migrationBuilder . CreateTable (
											"DbUsers" ,
											table => new { Guid = table . Column <Guid> ( nullable : false ) } ,
											constraints : table =>
														{
															table . PrimaryKey ( "PK_DbUsers" , x => x . Guid ) ;
														} ) ;

			migrationBuilder . CreateTable (
											"DbGroupMembers" ,
											table => new
													{
														Group  = table . Column <Guid> ( nullable : false ) ,
														Member = table . Column <Guid> ( nullable : false )
													} ,
											constraints : table =>
														{
															table . PrimaryKey (
																				"PK_DbGroupMembers" ,
																				x => new { x . Group , x . Member } ) ;
															table . ForeignKey (
																				"FK_DbGroupMembers_DbGroups_Group" ,
																				x => x . Group ,
																				"DbGroups" ,
																				"Guid" ,
																				onDelete : ReferentialAction .
																					Cascade ) ;
														} ) ;

			migrationBuilder . CreateTable (
											"DbProperties" ,
											table => new
													{
														Owner      = table . Column <Guid> ( nullable : false ) ,
														Name       = table . Column <string> ( nullable : false ) ,
														Value      = table . Column <string> ( nullable : true ) ,
														Permission = table . Column <string> ( nullable : true )
													} ,
											constraints : table =>
														{
															table . PrimaryKey (
																				"PK_DbProperties" ,
																				x => new { x . Owner , x . Name } ) ;
															table . ForeignKey (
																				"FK_DbProperties_DbDirectoryServices_Owner" ,
																				x => x . Owner ,
																				"DbDirectoryServices" ,
																				"Guid" ,
																				onDelete : ReferentialAction .
																					Cascade ) ;
															table . ForeignKey (
																				"FK_DbProperties_DbGroups_Owner" ,
																				x => x . Owner ,
																				"DbGroups" ,
																				"Guid" ,
																				onDelete : ReferentialAction .
																					Cascade ) ;
															table . ForeignKey (
																				"FK_DbProperties_DbLoginServices_Owner" ,
																				x => x . Owner ,
																				"DbLoginServices" ,
																				"Guid" ,
																				onDelete : ReferentialAction .
																					Cascade ) ;
															table . ForeignKey (
																				"FK_DbProperties_DbUsers_Owner" ,
																				x => x . Owner ,
																				"DbUsers" ,
																				"Guid" ,
																				onDelete : ReferentialAction .
																					Cascade ) ;
														} ) ;
		}

		protected override void Down ( MigrationBuilder migrationBuilder )
		{
			migrationBuilder . DropTable ( "DbGroupMembers" ) ;

			migrationBuilder . DropTable ( "DbProperties" ) ;

			migrationBuilder . DropTable ( "DbDirectoryServices" ) ;

			migrationBuilder . DropTable ( "DbGroups" ) ;

			migrationBuilder . DropTable ( "DbLoginServices" ) ;

			migrationBuilder . DropTable ( "DbUsers" ) ;
		}

	}

}
