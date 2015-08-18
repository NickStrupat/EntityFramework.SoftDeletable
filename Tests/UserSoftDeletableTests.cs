using System;
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

				var nick = new SpecialPerson(getUserIdFunc) { Name = "Nick" };
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

				var ned = new SpecialPerson(getUserIdFunc) { Name = "Ned" };
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
	}
}