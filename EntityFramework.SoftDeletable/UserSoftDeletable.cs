using System;

namespace EntityFramework.SoftDeletable {
    public abstract class UserSoftDeletable<TUserId> : SoftDeletable, IUserSoftDeletable<TUserId> {
        public TUserId DeletedById { get; set; }

        public static Func<IUserSoftDeletable<TUserId>, TUserId> CurrentUserIdFunc { internal get; set; }
    }
}