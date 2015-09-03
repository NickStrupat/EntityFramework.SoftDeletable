using System;
using System.Linq.Expressions;

namespace EntityFramework.VersionedSoftDeletable {
	internal static class FilterExpression<TVersionedSoftDeletable> where TVersionedSoftDeletable : IVersionedSoftDeletable {
		public static Expression<Func<TVersionedSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value;
	}

	internal static class FilterExpression<TVersionedUserSoftDeletable, TUserId, TVersionedUserDeleted>
		where TVersionedUserSoftDeletable : IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>
	{
		public static Expression<Func<TVersionedUserSoftDeletable, Boolean>> IsNotDeletedExpression = GetIsNotDeletedExpression(); //x => !x.Deleted.Value.IsDeleted;

		private static Expression<Func<TVersionedUserSoftDeletable, Boolean>> GetIsNotDeletedExpression() {
			var vusdType = typeof(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>);
			var deleted = vusdType.GetProperty(nameof(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>.Deleted)).GetGetMethod();

			var innerTypes = new VersionedUserSoftDeletableInnerTypes<TVersionedUserSoftDeletable>();
			var value = innerTypes.VersionedBaseType.GetProperty(nameof(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>.Deleted)).GetGetMethod();
			var isDeleted = innerTypes.UserDeletedType.GetProperty(nameof(UserDeleted<Object>.IsDeleted)).GetGetMethod();

			var expression = Expression.Parameter(vusdType, "x");
			return Expression.Lambda<Func<TVersionedUserSoftDeletable, Boolean>>(
				Expression.Not(
					Expression.Property(
						Expression.Property(
							Expression.Property(
								expression,
								deleted
							),
							value
						),
						isDeleted
					)
				), expression);
		}
	}
}