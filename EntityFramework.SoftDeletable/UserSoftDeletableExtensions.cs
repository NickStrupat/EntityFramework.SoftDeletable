using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public static class UserSoftDeletableExtensions {
        internal class SetterCache<TUserSoftDeletable, TUserId> where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
            public static Action<TUserSoftDeletable, TUserId> DeletedPropertySetter = PropertyReflection.GetValueSetter<TUserSoftDeletable, TUserId>(typeof(TUserSoftDeletable).GetProperty("DeletedById"));
        }

        public static void InitializeUserSoftDeletable<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable)
            where TUserSoftDeletable : class, IUserSoftDeletable<TUserId>
        {
            userSoftDeletable.InitializeSoftDeletableUpdating();
            userSoftDeletable.Triggers().Deleting += e =>
            {
                e.Entity.SetUserDeleted<TUserSoftDeletable, TUserId>(isDeleted: true);
                e.Cancel();
            };
        }

        public static void SoftDelete<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable)
            where TUserSoftDeletable : class, IUserSoftDeletable<TUserId>
        {
            userSoftDeletable.SetUserDeleted<TUserSoftDeletable, TUserId>(isDeleted: true);
        }

        public static void Restore<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable)
            where TUserSoftDeletable : class, IUserSoftDeletable<TUserId>
        {
            userSoftDeletable.SetUserDeleted<TUserSoftDeletable, TUserId>(isDeleted: false);
        }

        private static void SetUserDeleted<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable, Boolean isDeleted)
            where TUserSoftDeletable : class, IUserSoftDeletable<TUserId>
        {
            SetterCache<TUserSoftDeletable, TUserId>.DeletedPropertySetter(userSoftDeletable, userSoftDeletable.CurrentUserIdFunc());
            //userSoftDeletable.DeletedByIdPropertySetter(UserSoftDeletable<TUserId>.CurrentUserIdFunc(userSoftDeletable));
            //userSoftDeletable.DeletedById = UserSoftDeletable<TUserId>.CurrentUserIdFunc(userSoftDeletable);
            userSoftDeletable.SetDeleted(isDeleted);
        }
    }
}