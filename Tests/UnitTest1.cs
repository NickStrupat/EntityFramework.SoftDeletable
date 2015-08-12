using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using EntityFramework.Filters;
using EntityFramework.SoftDeletable;
using EntityFramework.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void TestMethod1() {
		}
	}
	public class Person : SoftDeletable {
		public Int64 Id { get; protected set; }
		public String Name { get; set; }
	}

	public class SpecialPerson : UserSoftDeletable<Int64> {
		public Int64 Id { get; protected set; }
		public String Name { get; set; }

		public override Int64 GetCurrentUserId() {
			return 42;
		}
	}

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
}
