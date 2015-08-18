using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using EntityFramework.Filters;
using EntityFramework.SoftDeletable;
using EntityFramework.Triggers;

namespace Tests {
	public class Context : DbContextWithTriggers {
		public DbSet<Person> People { get; set; }
		public DbSet<SpecialPerson> SpecialPeople { get; set; }

		public Context() {
			this.EnableSoftDeletableFilter();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			DbInterception.Add(new FilterInterceptor());
			modelBuilder.Conventions.AddSoftDeletableFilter();
			base.OnModelCreating(modelBuilder);
		}
	}

	public class Person : SoftDeletable {
		public Int64 Id { get; private set; }
		public String Name { get; set; }
	}

	public class SpecialPerson : UserSoftDeletable<Int64> {
		public Int64 Id { get; private set; }
		public String Name { get; set; }

		public Func<Int64> CurrentUserIdFunc { get; set; }
		public override Int64 GetCurrentUserId() {
			return CurrentUserIdFunc();
		}

		public SpecialPerson(Func<Int64> getUserIdFunc) {
			CurrentUserIdFunc = getUserIdFunc;
		}
	}
}