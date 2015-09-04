using System;
using System.Linq;
using EntityFramework.Extensions;
using EntityFramework.SoftDeletable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class SoftDeletableTests {
		[TestMethod]
		public void ExplicitSoftDelete() {
			using (var context = new Context()) {
				var nick = new Person { Name = "Nick" };
				context.People.Add(nick);
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());

				nick.SoftDelete();
				Assert.IsNotNull(nick.Deleted);
				Assert.IsTrue(nick.IsDeleted());

				nick.Restore();
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());

				context.SaveChanges();
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());
			}
		}

		[TestMethod]
		public void ImplicitSoftDelete() {
			using (var context = new Context()) {
				var ned = new Person { Name = "Ned" };
				context.People.Add(ned);
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());

				context.SaveChanges();
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());

				context.People.Remove(ned);
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());

				context.SaveChanges();
				Assert.IsNotNull(ned.Deleted);
				Assert.IsTrue(ned.IsDeleted());
			}
		}

		[TestMethod]
		public void GeneralFilter() {
			using (var context = new Context()) {
				var nick = new Person { Name = "Nick" };
				context.People.Add(nick);
				nick.SoftDelete();
				context.SaveChanges();
                var a = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(a);

				context.DisableSoftDeletableFilter();
				var b = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b);

				context.EnableSoftDeletableFilter();
				var c = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(c);
			}
		}

		[TestMethod]
		public void ZombieFilter() {
			using (var context = new Context()) {
				var nick = new Person { Name = "Nick" };
				context.People.Add(nick);
				nick.SoftDelete();

				var zack = new Zombie { Name = "Zack" };
				context.Zombies.Add(zack);
				zack.SoftDelete();

				context.SaveChanges();

				context.DisableSoftDeletableFilter<Zombie>();
				var a = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(a);
				var w = context.Zombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(w);

				context.DisableSoftDeletableFilter();
				var b = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b);
				var y = context.Zombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNotNull(y);

				context.EnableSoftDeletableFilter<Zombie>();
				var b2 = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b2);
				var y2 = context.Zombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(y2);

				context.EnableSoftDeletableFilter();
				var c = context.People.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(c);
				var z = context.Zombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(z);
			}
		}
	}
}
