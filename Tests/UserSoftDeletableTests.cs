using System;
using System.Linq;
using EntityFramework.SoftDeletable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class UserSoftDeletableTests {
		[TestMethod]
		public void ExplicitSoftDelete() {
			using (var context = new Context()) {
				var userIdGetterCount = 0;
				Func<Int64> getUserIdFunc = () => ++userIdGetterCount;

				var nick = new SpecialPerson("Nick", getUserIdFunc);
				context.SpecialPeople.Add(nick);
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				nick.SoftDelete();
				Assert.IsNotNull(nick.Deleted);
				Assert.IsTrue(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 1);

				nick.Restore();
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);

				context.SaveChanges();
				Assert.IsNull(nick.Deleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);
			}
		}

		[TestMethod]
		public void ImplicitSoftDelete() {
			using (var context = new Context()) {
				var userIdGetterCount = 0;
				Func<Int64> getUserIdFunc = () => ++userIdGetterCount;

				var ned = new SpecialPerson("Ned", getUserIdFunc);
				context.SpecialPeople.Add(ned);
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.SaveChanges();
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.SpecialPeople.Remove(ned);
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.SaveChanges();
				Assert.IsNotNull(ned.Deleted);
				Assert.IsTrue(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 1);

				ned.Restore();
				context.SaveChanges();
				Assert.IsNull(ned.Deleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);
			}
		}

		[TestMethod]
		public void GeneralFilter() {
			using (var context = new Context()) {
				var nick = new SpecialPerson("Nick", () => 42);
				context.SpecialPeople.Add(nick);
				nick.SoftDelete();
				context.SaveChanges();
				var a = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(a);

				context.DisableSoftDeletableFilter();
				var b = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b);

				context.EnableSoftDeletableFilter();
				var c = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(c);
			}
		}

		[TestMethod]
		public void ZombieFilter() {
			using (var context = new Context()) {
				Func<Int64> getUserIdFunc = () => 42;

				var nick = new SpecialPerson("Nick", getUserIdFunc);
				context.SpecialPeople.Add(nick);
				nick.SoftDelete();

				var zack = new SpecialZombie("Zack", getUserIdFunc);
				context.SpecialZombies.Add(zack);
				zack.SoftDelete();

				context.SaveChanges();

				context.DisableSoftDeletableFilter<SpecialZombie>();
				var a = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(a);
				var w = context.SpecialZombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(w);

				context.DisableSoftDeletableFilter();
				var b = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b);
				var y = context.SpecialZombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNotNull(y);

				context.EnableSoftDeletableFilter<SpecialZombie>();
				var b2 = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNotNull(b2);
				var y2 = context.SpecialZombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(y2);

				context.EnableSoftDeletableFilter();
				var c = context.SpecialPeople.SingleOrDefault(x => x.Id == nick.Id);
				Assert.IsNull(c);
				var z = context.SpecialZombies.SingleOrDefault(x => x.Id == zack.Id);
				Assert.IsNull(z);
			}
		}
	}
}