using System;
using System.Linq.Expressions;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedUserSoftDeletable : VersionedUserSoftDeletable<String, VersionedUserDeleted>, IVersionedUserSoftDeletable {
		//internal new static Expression<Func<IVersionedUserSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value.IsDeleted;
	}

	public abstract class VersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> : IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> {
		public TVersionedUserDeleted Deleted { get; internal set; }
		public abstract TUserId GetCurrentUserId();

		protected VersionedUserSoftDeletable() {
			this.InitializeVersionedUserSoftDeletable();
		}
	}
}