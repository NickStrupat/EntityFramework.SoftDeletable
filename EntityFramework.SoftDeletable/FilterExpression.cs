using System;
using System.Linq.Expressions;

namespace EntityFramework.SoftDeletable {
	internal static class FilterExpression<TSoftDeletable> where TSoftDeletable : ISoftDeletable {
		public static Expression<Func<TSoftDeletable, Boolean>> IsNotDeletedExpression = x => x.Deleted == null;
	}
}