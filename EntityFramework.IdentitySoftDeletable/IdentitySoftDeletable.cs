using System;
using EntityFramework.SoftDeletable;
using Microsoft.AspNet.Identity;

namespace EntityFramework.IdentitySoftDeletable {
	public abstract class IdentitySoftDeletable<T> : UserSoftDeletable<T> where T : IConvertible {
		public sealed override T GetCurrentUserId() {
			return System.Web.HttpContext.Current.User.Identity.GetUserId<T>();
		}
	}
}
