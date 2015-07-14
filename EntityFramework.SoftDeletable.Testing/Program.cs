using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using EntityFramework.Filters;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable.Testing {
    class Program {
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

        static void Main(String[] args) {
            using (var context = new Context()) {
                var nick = new Person { Name = "Nick" };
                context.People.Add(nick);
                context.SaveChanges();
                context.People.Remove(nick);
                context.SaveChanges();
                nick.Restore();
                context.SaveChanges();

                var ferenc = new SpecialPerson {Name = "Ferenc"};
                context.SpecialPeople.Add(ferenc);
                context.SaveChanges();
                context.SpecialPeople.Remove(ferenc);
                context.SaveChanges();
                ferenc.Restore();
                context.SaveChanges();
            }
        }
    }
}
