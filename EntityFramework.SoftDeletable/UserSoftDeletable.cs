using System;

namespace EntityFramework.SoftDeletable {
    public abstract class UserSoftDeletable<TUserId> : IUserSoftDeletable<TUserId> {
        public DateTime? Deleted { get; internal set; }
        public TUserId DeletedById { get; internal set; }

        public Func<TUserId> CurrentUserIdFunc { get; set; }

        protected UserSoftDeletable() {
            this.InitializeUserSoftDeletable<UserSoftDeletable<TUserId>, TUserId>();
        }
    }
}