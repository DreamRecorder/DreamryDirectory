using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Services.Logic.Storage;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace DreamRecorder.Directory.Services.ApiService
{

	public class DirectoryDatabaseStorage : DbContext, IDirectoryDatabaseStorage
	{
		public DbSet<DbEntity> DbEntities { get; set; }

		public DbSet<DbGroupMember> DbGroupMembers { get; set; }

		public DbSet<DbProperty> DbProperties { get; set; }

		public DbSet<DbPermissionGroup> DbPermissionGroups { get; set; }

		public void Save() { SaveChanges(); }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(
											"Data Source=fesqlserver.dreamry.org;Initial Catalog=DreamryDirectory;Integrated Security=True");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{



			modelBuilder.Entity<DbEntity>().HasKey(entity => entity.Guid);
			modelBuilder.Entity<DbEntity>().
							HasMany<DbProperty>().
							WithOne().
							HasForeignKey(prop => prop.Target);

			modelBuilder.Entity<DbPermissionGroup>().HasKey(permissionGroup => permissionGroup.Guid);

			modelBuilder.Entity<DbGroupMember>().
							HasKey(
									groupMember => new
									{
										Group = groupMember.GroupGuid,
										Member = groupMember.MemberGuid,
									});
			modelBuilder.Entity<DbGroupMember>().HasOne<DbEntity>().WithMany().HasForeignKey(groupMember => groupMember.GroupGuid);
			modelBuilder.Entity<DbGroupMember>().HasOne<DbEntity>().WithMany().HasForeignKey(groupMember => groupMember.MemberGuid);

			modelBuilder.Entity<DbProperty>().
			HasKey(property => new { Target = property.Target, Name = property.Name, });
			modelBuilder.Entity<DbProperty>().
							HasOne<DbPermissionGroup>().
							WithMany().
							HasForeignKey(prop => prop.Permission);
		}

	}

}
