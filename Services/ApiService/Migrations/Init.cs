using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamRecorder.Directory.Services.ApiService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbDirectoryServices",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbDirectoryServices", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbGroups",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroups", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbLoginServices",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLoginServices", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbUsers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUsers", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbGroupMembers",
                columns: table => new
                {
                    Group = table.Column<Guid>(nullable: false),
                    Member = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroupMembers", x => new { x.Group, x.Member });
                    table.ForeignKey(
                        name: "FK_DbGroupMembers_DbGroups_Group",
                        column: x => x.Group,
                        principalTable: "DbGroups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbProperties",
                columns: table => new
                {
                    Owner = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProperties", x => new { x.Owner, x.Name });
                    table.ForeignKey(
                        name: "FK_DbProperties_DbDirectoryServices_Owner",
                        column: x => x.Owner,
                        principalTable: "DbDirectoryServices",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbGroups_Owner",
                        column: x => x.Owner,
                        principalTable: "DbGroups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbLoginServices_Owner",
                        column: x => x.Owner,
                        principalTable: "DbLoginServices",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbUsers_Owner",
                        column: x => x.Owner,
                        principalTable: "DbUsers",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbGroupMembers");

            migrationBuilder.DropTable(
                name: "DbProperties");

            migrationBuilder.DropTable(
                name: "DbDirectoryServices");

            migrationBuilder.DropTable(
                name: "DbGroups");

            migrationBuilder.DropTable(
                name: "DbLoginServices");

            migrationBuilder.DropTable(
                name: "DbUsers");
        }
    }
}
