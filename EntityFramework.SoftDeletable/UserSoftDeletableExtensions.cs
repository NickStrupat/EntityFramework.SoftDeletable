using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public static class UserSoftDeletableExtensions {
        private const String DeletedChangedByIdPropertyName = "DeletedChangedById";

        internal static class SetterCache<TUserSoftDeletable, TUserId> where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
            public static readonly Action<TUserSoftDeletable, TUserId> DeletedChangedByIdPropertySetter = PropertyReflection.GetValueSetter<TUserSoftDeletable, TUserId>(typeof(TUserSoftDeletable).GetProperty(DeletedChangedByIdPropertyName));
        }

        private static DateTime? GetOriginalDeletedValue<TUserSoftDeletable, TUserId>(IBeforeEntry<TUserSoftDeletable> entry) where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
            return (DateTime?)entry.Context.Entry(entry.Entity).OriginalValues[SoftDeletableExtensions.DeletedPropertyName];
        }

        public static void InitializeUserSoftDeletable<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable) where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
            userSoftDeletable.Triggers().Deleting += e => {
                var originalDeletedValue = GetOriginalDeletedValue<TUserSoftDeletable, TUserId>(e);
                e.Entity.SetDeleted(isDeleted: true);
                if (originalDeletedValue == null)
                    SetterCache<TUserSoftDeletable, TUserId>.DeletedChangedByIdPropertySetter(e.Entity, e.Entity.GetCurrentUserId());
                e.Cancel();
            };
            userSoftDeletable.Triggers().Updating += e => {
                if (e.Entity.IsDeleted())
                    throw new SoftDeletableModifiedWhileDeletedException();
                if (GetOriginalDeletedValue<TUserSoftDeletable, TUserId>(e) != null)
                    SetterCache<TUserSoftDeletable, TUserId>.DeletedChangedByIdPropertySetter(e.Entity, e.Entity.GetCurrentUserId());
            };
        }
    }
}