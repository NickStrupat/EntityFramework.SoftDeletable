namespace EntityFramework.SoftDeletable {
	public interface IUserSoftDeletable : ISoftDeletable { }

    public interface IUserSoftDeletable<TUserId> : IUserSoftDeletable {
	    TUserId DeletedById { get; }
	    TUserId GetCurrentUserId();
    }
}