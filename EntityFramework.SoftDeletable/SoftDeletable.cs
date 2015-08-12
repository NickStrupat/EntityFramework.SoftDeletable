using System;
using System.Linq.Expressions;

namespace EntityFramework.SoftDeletable {
	public abstract class SoftDeletable : ISoftDeletable {
		public DateTime? Deleted { get; private set; }

		protected SoftDeletable() {
			this.InitializeSoftDeletable();
		}

		internal static Expression<Func<ISoftDeletable, Boolean>> IsNotDeletedExpression = x => x.Deleted == null;
	}
}