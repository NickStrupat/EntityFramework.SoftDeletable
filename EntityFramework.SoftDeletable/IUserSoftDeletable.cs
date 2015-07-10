using System;

namespace EntityFramework.SoftDeletable {
    public interface IUserSoftDeletable<TUserId> : ISoftDeletable {
        TUserId DeletedById { get; }
        
        Func<TUserId> CurrentUserIdFunc { get; set; }
    }
}