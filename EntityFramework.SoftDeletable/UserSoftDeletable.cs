using System;

namespace EntityFramework.SoftDeletable {
    public abstract class UserSoftDeletable<TUserId> : IUserSoftDeletable<TUserId> {
	    public DateTime? Deleted { get; protected set; }
        public TUserId DeletedById { get; set; }

        protected UserSoftDeletable() {
			this.InitializeSoftDeletable();
		}

        public static Func<IUserSoftDeletable<TUserId>, TUserId> CurrentUserIdFunc { internal get; set; }
    }
}