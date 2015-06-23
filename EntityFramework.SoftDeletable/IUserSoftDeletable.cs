using System;

namespace EntityFramework.SoftDeletable {
    public interface IUserSoftDeletable<TUserId> : ISoftDeletable {
        TUserId DeletedById { get; }

        Action<TUserId> DeletedByIdPropertySetter { get; set; }
    }
}