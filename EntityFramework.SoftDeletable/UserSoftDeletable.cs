namespace EntityFramework.SoftDeletable {
	public abstract class UserSoftDeletable<TUserId> : SoftDeletable, IUserSoftDeletable<TUserId> {
		public TUserId DeletedChangedById { get; internal set; }

		public abstract TUserId GetCurrentUserId();
	}
}