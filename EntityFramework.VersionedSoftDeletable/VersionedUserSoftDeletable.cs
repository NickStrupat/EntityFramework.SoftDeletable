using System;
using System.Linq.Expressions;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedUserSoftDeletable : IVersionedUserSoftDeletable {
		public VersionedUserDeleted Deleted { get; internal set; }
		public abstract String GetCurrentUserId();

		internal static Expression<Func<IVersionedUserSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value.IsDeleted;
	}
}