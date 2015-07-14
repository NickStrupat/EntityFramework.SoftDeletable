namespace EntityFramework.SoftDeletable {
	public interface IUserSoftDeletable : ISoftDeletable { }

    public interface IUserSoftDeletable<TUserId> : IUserSoftDeletable {
	    TUserId DeletedChangedById { get; }
	    TUserId GetCurrentUserId();
    }
}