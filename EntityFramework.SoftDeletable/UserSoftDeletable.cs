using System;

namespace EntityFramework.SoftDeletable {
    public abstract class UserSoftDeletable<TUserId> : IUserSoftDeletable<TUserId> {
        public DateTime? Deleted { get; private set; }
        public TUserId DeletedById { get; set; }

        public Action<DateTime?> DeletedPropertySetter { get { return x => Deleted = x; } }
        public Action<TUserId> DeletedByIdPropertySetter { get { return x => DeletedById = x; } }

        protected UserSoftDeletable() {
            this.InitializeSoftDeletable();
        }
        
        public static Func<IUserSoftDeletable<TUserId>, TUserId> CurrentUserIdFunc { internal get; set; }
    }
}