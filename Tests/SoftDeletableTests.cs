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
	}
}
