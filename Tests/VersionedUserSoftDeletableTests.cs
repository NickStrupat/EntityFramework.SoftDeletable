using System;
using EntityFramework.VersionedSoftDeletable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class VersionedUserSoftDeletableTests {
		[TestMethod]
		public void ExplicitSoftDelete() {
			using (var context = new Context()) {
				var userIdGetterCount = 0;
				Func<String> getUserIdFunc = () => (++userIdGetterCount).ToString();

				var nick = new VuPerson { Name = "Nick", CurrentUserIdFunc = getUserIdFunc };
				context.VuPeople.Add(nick);
				Assert.IsFalse(nick.Deleted.Value.IsDeleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				nick.SoftDelete();
				Assert.IsTrue(nick.Deleted.Value.IsDeleted);
				Assert.IsTrue(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 1);

				nick.Restore();
				Assert.IsFalse(nick.Deleted.Value.IsDeleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);

				context.SaveChanges();
				Assert.IsFalse(nick.Deleted.Value.IsDeleted);
				Assert.IsFalse(nick.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);
			}
		}

		[TestMethod]
		public void ImplicitSoftDelete() {
			using (var context = new Context()) {
				var userIdGetterCount = 0;
				Func<String> getUserIdFunc = () => (++userIdGetterCount).ToString();

				var ned = new VuPerson { Name = "Ned", CurrentUserIdFunc = getUserIdFunc };
				context.VuPeople.Add(ned);
				Assert.IsFalse(ned.Deleted.Value.IsDeleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.SaveChanges();
				Assert.IsFalse(ned.Deleted.Value.IsDeleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.VuPeople.Remove(ned);
				Assert.IsFalse(ned.Deleted.Value.IsDeleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 0);

				context.SaveChanges();
				Assert.IsTrue(ned.Deleted.Value.IsDeleted);
				Assert.IsTrue(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 1);

				ned.Restore();
				context.SaveChanges();
				Assert.IsFalse(ned.Deleted.Value.IsDeleted);
				Assert.IsFalse(ned.IsDeleted());
				Assert.IsTrue(userIdGetterCount == 2);
			}
		}
	}
}