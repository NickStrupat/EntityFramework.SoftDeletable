namespace EntityFramework.SoftDeletable {
    public interface IUserSoftDeletable<TUserId> : ISoftDeletable {
        TUserId DeletedChangedById { get; }
        TUserId GetCurrentUserId();
    }
}