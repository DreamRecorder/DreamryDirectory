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
    [Migration("20201106135710_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbDirectoryService", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbDirectoryServices");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbGroup", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbGroups");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbGroupMember", b =>
                {
                    b.Property<Guid>("GroupGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MemberGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GroupGuid", "MemberGuid");

                    b.ToTable("DbGroupMembers");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbLoginService", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbLoginServices");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbProperty", b =>
                {
                    b.Property<Guid>("Target")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Permission")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Target", "Name");

                    b.ToTable("DbProperties");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbUser", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.ToTable("DbUsers");
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbGroupMember", b =>
                {
                    b.HasOne("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbGroup", null)
                        .WithMany("Members")
                        .HasForeignKey("GroupGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbProperty", b =>
                {
                    b.HasOne("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbDirectoryService", null)
                        .WithMany("Proprieties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbGroup", null)
                        .WithMany("Proprieties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbLoginService", null)
                        .WithMany("Proprieties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DreamRecorder.Directory.Services.ApiService.DirectoryDatabaseStorage+DbUser", null)
                        .WithMany("Proprieties")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
