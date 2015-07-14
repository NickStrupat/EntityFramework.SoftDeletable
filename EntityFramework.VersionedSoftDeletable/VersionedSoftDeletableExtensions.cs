using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable
{
    public static class VersionedSoftDeletableExtensions
    {
        public static void InitializeVersionedSoftDeletable(this IVersionedSoftDeletable versionedSoftDeletable)
        {
            versionedSoftDeletable.InitializeVersionedProperties();
            versionedSoftDeletable.Triggers().Deleting += e =>
            {
                e.Entity.SetDeleted(isDeleted: true);
                e.Cancel();
            };
            versionedSoftDeletable.Triggers().Updating += e =>
            {
                if (e.Entity.IsDeleted())
                    throw new SoftDeletable.SoftDeletableModifiedWhileDeletedException();
            };
        }

        public static Boolean IsDeleted(this IVersionedSoftDeletable softDeletable)
        {
            return softDeletable.Deleted.Value;
        }

        public static void SoftDelete<TVersionedSoftDeletable>(this TVersionedSoftDeletable softDeletable) where TVersionedSoftDeletable : class, IVersionedSoftDeletable
        {
            softDeletable.SetDeleted(isDeleted: true);
        }

        public static void Restore<TVersionedSoftDeletable>(this TVersionedSoftDeletable softDeletable) where TVersionedSoftDeletable : class, IVersionedSoftDeletable
        {
            softDeletable.SetDeleted(isDeleted: false);
        }

        internal static void SetDeleted<TVersionedSoftDeletable>(this TVersionedSoftDeletable softDeletable, Boolean isDeleted) where TVersionedSoftDeletable : class, IVersionedSoftDeletable
        {
            softDeletable.Deleted.Value = isDeleted;
        }
    }
}
