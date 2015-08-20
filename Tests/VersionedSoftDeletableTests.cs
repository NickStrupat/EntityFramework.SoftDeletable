﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.VersionedSoftDeletable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class VersionedSoftDeletableTests {
		[TestMethod]
		public void ExplicitSoftDelete() {
			using (var context = new Context()) {
				var nick = new VPerson { Name = "Nick" };
				context.VPeople.Add(nick);
				Assert.IsFalse(nick.Deleted.Value);
				Assert.IsFalse(nick.IsDeleted());

				nick.SoftDelete();
				Assert.IsTrue(nick.Deleted.Value);
				Assert.IsTrue(nick.IsDeleted());
				Assert.IsFalse(nick.Deleted.LocalVersions.Single().Value);

				nick.Restore();
				Assert.IsFalse(nick.Deleted.Value);
				Assert.IsFalse(nick.IsDeleted());

				context.SaveChanges();
				Assert.IsFalse(nick.Deleted.Value);
				Assert.IsFalse(nick.IsDeleted());
			}
		}

		[TestMethod]
		public void ImplicitSoftDelete() {
			using (var context = new Context()) {
				var ned = new VPerson { Name = "Ned" };
				context.VPeople.Add(ned);
				Assert.IsFalse(ned.Deleted.Value);
				Assert.IsFalse(ned.IsDeleted());

				context.SaveChanges();
				Assert.IsFalse(ned.Deleted.Value);
				Assert.IsFalse(ned.IsDeleted());

				context.VPeople.Remove(ned);
				Assert.IsFalse(ned.Deleted.Value);
				Assert.IsFalse(ned.IsDeleted());

				context.SaveChanges();
				Assert.IsTrue(ned.Deleted.Value);
				Assert.IsTrue(ned.IsDeleted());
				Assert.IsFalse(ned.Deleted.LocalVersions.Single().Value);
			}
		}
	}
}