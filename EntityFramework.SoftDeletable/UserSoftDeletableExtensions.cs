using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public static class UserSoftDeletableExtensions {
		public static void InitializeSoftDeletable<TUserId>(this IUserSoftDeletable<TUserId> userSoftDeletable) {
			userSoftDeletable.Triggers().Deleting += e => {
				                                     e.Entity.SetDeleted(isDeleted: true);
				                                     e.Cancel();
			                                     };
			userSoftDeletable.Triggers().Updating += e => {
				                                     if (e.Entity.IsDeleted())
					                                     throw new SoftDeletableModifiedWhileDeletedException();
			                                     };
        }

        public static void Restore<TUserId>(this IUserSoftDeletable<TUserId> userSoftDeletable) {
            userSoftDeletable.SetDeleted(isDeleted: false);
        }

        private static void SetDeleted<TUserId>(this IUserSoftDeletable<TUserId> userSoftDeletable, Boolean isDeleted) {
            userSoftDeletable.DeletedById = UserSoftDeletable<TUserId>.CurrentUserIdFunc(userSoftDeletable);
            SoftDeletableExtensions.SetDeleted(userSoftDeletable, isDeleted);
        }
	}
}
