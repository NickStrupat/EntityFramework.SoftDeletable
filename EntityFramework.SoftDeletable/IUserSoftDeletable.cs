namespace EntityFramework.SoftDeletable {
    public interface IUserSoftDeletable<TUserId> : ISoftDeletable {
        TUserId DeletedById { get; set; }
    }
}