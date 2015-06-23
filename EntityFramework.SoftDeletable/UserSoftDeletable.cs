using System;

namespace EntityFramework.SoftDeletable {
    public abstract class UserSoftDeletable<TUserId> : SoftDeletable, IUserSoftDeletable<TUserId> {
        public TUserId DeletedById { get; set; }

        public Action<TUserId> DeletedByIdPropertySetter { get { return x => DeletedById = x; } set { throw new NotImplementedException(); } }
        
        public static Func<IUserSoftDeletable<TUserId>, TUserId> CurrentUserIdFunc { internal get; set; }
    }
}