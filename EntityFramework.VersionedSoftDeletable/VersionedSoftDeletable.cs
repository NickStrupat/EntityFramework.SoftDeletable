using System;
using System.Linq.Expressions;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedSoftDeletable : IVersionedSoftDeletable {
		public VersionedBoolean Deleted { get; internal set; }

		protected VersionedSoftDeletable() {
			this.InitializeVersionedSoftDeletable();
		}

		internal static Expression<Func<IVersionedSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value;
	}
}