using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public static class UserSoftDeletableExtensions {
	    private const String DeletedByIdPropertyName = "DeletedById";

		internal static class SetterCache<TUserId> {
			public static Action<IUserSoftDeletable<TUserId>, TUserId> DeletedByIdPropertySetter = PropertyReflection.GetValueSetter<IUserSoftDeletable<TUserId>, TUserId>(typeof(IUserSoftDeletable<TUserId>).GetProperty(DeletedByIdPropertyName));
		}

        public static void InitializeUserSoftDeletable<TUserId>(this IUserSoftDeletable<TUserId> userSoftDeletable) {
			userSoftDeletable.Triggers().Deleting += e => {
				var originalDeletedValue = (DateTime?) e.Context.Entry(e.Entity).OriginalValues[SoftDeletableExtensions.DeletedPropertyName];
				e.Entity.SetDeleted(isDeleted: true);
				if (originalDeletedValue == null)
					SetterCache<TUserId>.DeletedByIdPropertySetter(e.Entity, e.Entity.GetCurrentUserId());
				e.Cancel();
			};
            userSoftDeletable.Triggers().Updating += e => {
				if (e.Entity.IsDeleted())
					throw new SoftDeletableModifiedWhileDeletedException();

            };
        }
    }
}