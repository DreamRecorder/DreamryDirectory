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
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbDirectoryServices", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbGroups",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroups", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbLoginServices",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLoginServices", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbPermissionGroups",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbPermissionGroups", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbServices",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbServices", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbUsers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUsers", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbGroupMembers",
                columns: table => new
                {
                    GroupGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroupMembers", x => new { x.GroupGuid, x.MemberGuid });
                    table.ForeignKey(
                        name: "FK_DbGroupMembers_DbGroups_GroupGuid",
                        column: x => x.GroupGuid,
                        principalTable: "DbGroups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbProperties",
                columns: table => new
                {
                    Target = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermissionGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProperties", x => new { x.Target, x.Name });
                    table.ForeignKey(
                        name: "FK_DbProperties_DbDirectoryServices_Target",
                        column: x => x.Target,
                        principalTable: "DbDirectoryServices",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbGroups_Target",
                        column: x => x.Target,
                        principalTable: "DbGroups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbLoginServices_Target",
                        column: x => x.Target,
                        principalTable: "DbLoginServices",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbPermissionGroups_PermissionGuid",
                        column: x => x.PermissionGuid,
                        principalTable: "DbPermissionGroups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbServices_Target",
                        column: x => x.Target,
                        principalTable: "DbServices",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProperties_DbUsers_Target",
                        column: x => x.Target,
                        principalTable: "DbUsers",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbProperties_PermissionGuid",
                table: "DbProperties",
                column: "PermissionGuid");
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
                name: "DbPermissionGroups");

            migrationBuilder.DropTable(
                name: "DbServices");

            migrationBuilder.DropTable(
                name: "DbUsers");
        }
    }
}
