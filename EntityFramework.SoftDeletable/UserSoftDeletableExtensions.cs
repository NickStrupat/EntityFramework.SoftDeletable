using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
	public static class UserSoftDeletableExtensions {
		internal static class SetterCache<TUserSoftDeletable, TUserId> where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
			public static readonly Action<TUserSoftDeletable, TUserId> DeletedChangedByIdPropertySetter = PropertyReflection.GetValueSetter<TUserSoftDeletable, TUserId>(
				typeof(TUserSoftDeletable).GetProperty(nameof(IUserSoftDeletable<TUserId>.DeletedChangedById))
			);
		}

		public static void InitializeUserSoftDeletable<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable) where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
			userSoftDeletable.Triggers().Deleting += e => {
				if (e.GetOriginalDeletedValue() == null)
					SetterCache<TUserSoftDeletable, TUserId>.DeletedChangedByIdPropertySetter(e.Entity, e.Entity.GetCurrentUserId());
			};
			userSoftDeletable.InitializeSoftDeletable();
			userSoftDeletable.Triggers().Updating += e => {
				SetterCache<TUserSoftDeletable, TUserId>.DeletedChangedByIdPropertySetter(e.Entity, e.Entity.GetCurrentUserId());
			};
		}
	}
}