﻿// <auto-generated />
using System;
using DreamRecorder.Directory.Services.ApiService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DreamRecorder.Directory.Services.ApiService.Migrations
{
    [DbContext(typeof(DirectoryDatabaseStorage))]
    [Migration("20210512072220_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbDirectoryService", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbDirectoryServices");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbGroup", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbGroups");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbGroupMember", b =>
                {
                    b.Property<Guid>("GroupGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MemberGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GroupGuid", "MemberGuid");

                    b.ToTable("DbGroupMembers");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbLoginService", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbLoginServices");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbPermissionGroup", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("DbPermissionGroups");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbProperty", b =>
                {
                    b.Property<Guid>("Target")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PermissionGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Target", "Name");

                    b.HasIndex("PermissionGuid");

                    b.ToTable("DbProperties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbService", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbServices");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbUser", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbUsers");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbGroupMember", b =>
                {
                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbGroup", null)
                        .WithMany("Members")
                        .HasForeignKey("GroupGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbProperty", b =>
                {
                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbPermissionGroup", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbDirectoryService", null)
                        .WithMany("Properties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbGroup", null)
                        .WithMany("Properties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbLoginService", null)
                        .WithMany("Properties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbService", null)
                        .WithMany("Properties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.Logic.Storage.DbUser", null)
                        .WithMany("Properties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbDirectoryService", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbGroup", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbLoginService", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbService", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.Logic.Storage.DbUser", b =>
                {
                    b.Navigation("Properties");
                });
#pragma warning restore 612, 618
        }
    }
}