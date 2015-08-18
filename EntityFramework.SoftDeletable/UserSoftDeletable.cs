using System;

namespace EntityFramework.SoftDeletable {
	public abstract class UserSoftDeletable<TUserId> : SoftDeletable, IUserSoftDeletable<TUserId> {
		public TUserId DeletedChangedById { get; private set; }

		public abstract TUserId GetCurrentUserId();
	}
}