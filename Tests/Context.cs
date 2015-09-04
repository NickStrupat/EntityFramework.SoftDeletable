using System;
using System.Data.Entity;
using EntityFramework.SoftDeletable;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;
using EntityFramework.VersionedSoftDeletable;

namespace Tests {
	public class Context : DbContextWithTriggers, IBooleanVersions, IUserDeleteds {
		public DbSet<Person> People { get; set; }
		public DbSet<SpecialPerson> SpecialPeople { get; set; }
		public DbSet<VPerson> VPeople { get; set; }
		public DbSet<VSpecialPerson> VSpecialPeople { get; set; }

		public DbSet<Zombie> Zombies { get; set; }
		public DbSet<SpecialZombie> SpecialZombies { get; set; }
		public DbSet<VZombie> VZombies { get; set; }
		public DbSet<VSpecialZombie> VSpecialZombies { get; set; }

		static Context() {
			Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.AddSoftDeletableFilter();
			modelBuilder.AddSoftDeletableFilter<Zombie>();
			modelBuilder.AddSoftDeletableFilter<SpecialZombie>();
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

		protected SpecialPerson() { }
		public SpecialPerson(String name, Func<Int64> currentUserIdFunc) {
			Name = name;
			CurrentUserIdFunc = currentUserIdFunc;
		}

		public Func<Int64> CurrentUserIdFunc { get; }
		public override Int64 GetCurrentUserId() => CurrentUserIdFunc();
	}

	public class VPerson : VersionedSoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class VSpecialPerson : VersionedUserSoftDeletable {
		protected VSpecialPerson() { }
		public VSpecialPerson(String name, Func<String> currentUserIdFunc) {
			Name = name;
			CurrentUserIdFunc = currentUserIdFunc;
		}

		public Int64 Id { get; private set; }
		public String Name { get; set; }

		public Func<String> CurrentUserIdFunc { get; }
		public override String GetCurrentUserId() => CurrentUserIdFunc();
	}

	public class Zombie : SoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class SpecialZombie : UserSoftDeletable<Int64> {
		public Int64 Id { get; private set; }
		public String Name { get; set; }

		protected SpecialZombie() { }
		public SpecialZombie(String name, Func<Int64> currentUserIdFunc) {
			Name = name;
			CurrentUserIdFunc = currentUserIdFunc;
		}

		public Func<Int64> CurrentUserIdFunc { get; }
		public override Int64 GetCurrentUserId() => CurrentUserIdFunc();
	}

	public class VZombie : VersionedSoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class VSpecialZombie : VersionedUserSoftDeletable {
		protected VSpecialZombie() { }
		public VSpecialZombie(String name, Func<String> currentUserIdFunc) {
			Name = name;
			CurrentUserIdFunc = currentUserIdFunc;
		}

		public Int64 Id { get; private set; }
		public String Name { get; set; }

		public Func<String> CurrentUserIdFunc { get; }
		public override String GetCurrentUserId() => CurrentUserIdFunc();
	}
}