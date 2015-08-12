using System;

namespace EntityFramework.SoftDeletable {
	public abstract class UserSoftDeletable<TUserId> : IUserSoftDeletable<TUserId> {
		public DateTime? Deleted { get; private set; }
		public TUserId DeletedChangedById { get; private set; }

		public abstract TUserId GetCurrentUserId();

		protected UserSoftDeletable() {
			this.InitializeUserSoftDeletable<UserSoftDeletable<TUserId>, TUserId>();
		}
	}
}