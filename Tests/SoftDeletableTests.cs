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
		public void Filter() {
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
	}
}
