using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using EntityFramework.SoftDeletable;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;
using EntityFramework.VersionedSoftDeletable;

namespace Tests {
	public class MigrationsConfiguration : DbMigrationsConfiguration<Context> {
		public MigrationsConfiguration() {
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}
	}

	public class Context : DbContextWithTriggers, IBooleanVersions, IUserDeleteds {
		public DbSet<Person> People { get; set; }
		public DbSet<SpecialPerson> SpecialPeople { get; set; }
		public DbSet<VPerson> VPeople { get; set; }
		public DbSet<VuPerson> VuPeople { get; set; }

		public Context() {
			this.EnableSoftDeletableFilter();
			
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.AddSoftDeletableFilter();
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<BooleanVersion> BooleanVersions { get; set; }
		public DbSet<UserDeletedVersion> UserDeleteds { get; set; }
	}

	public class Person : SoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class SpecialPerson : UserSoftDeletable<Int64> {
		public Int64 Id { get; private set; }
		public String Name { get; set; }

		public Func<Int64> CurrentUserIdFunc { get; set; }
		public override Int64 GetCurrentUserId() => CurrentUserIdFunc();
	}

	public class VPerson : VersionedSoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class VuPerson : VersionedUserSoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }

		public Func<String> CurrentUserIdFunc { get; set; }
		public override String GetCurrentUserId() => CurrentUserIdFunc();
	}
}