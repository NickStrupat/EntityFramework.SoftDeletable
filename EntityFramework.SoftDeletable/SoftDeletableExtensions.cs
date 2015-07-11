using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public static class SoftDeletableExtensions {
        internal class SetterCache<TSoftDeletable> where TSoftDeletable : class, ISoftDeletable {
            public static Action<TSoftDeletable, DateTime?> DeletedPropertySetter = PropertyReflection.GetValueSetter<TSoftDeletable, DateTime?>(typeof(TSoftDeletable).GetProperty("Deleted"));
        }

        public static void InitializeSoftDeletable<TSoftDeletable>(this TSoftDeletable softDeletable)
            where TSoftDeletable : class, ISoftDeletable {
            softDeletable.Triggers().Deleting += e => {
                e.Entity.SetDeleted(isDeleted: true);
                e.Cancel();
            };
            softDeletable.InitializeSoftDeletableUpdating();
        }

        internal static void InitializeSoftDeletableUpdating(this ISoftDeletable softDeletable) {
            softDeletable.Triggers().Updating += e => {
                if (e.Entity.IsDeleted())
                    throw new SoftDeletableModifiedWhileDeletedException();
            };
        }

        public static Boolean IsDeleted(this ISoftDeletable softDeletable) {
            return softDeletable.Deleted != null;
        }

        public static void SoftDelete<TSoftDeletable>(this TSoftDeletable softDeletable) where TSoftDeletable : class, ISoftDeletable {
            softDeletable.SetDeleted(isDeleted: true);
        }

        public static void Restore<TSoftDeletable>(this TSoftDeletable softDeletable) where TSoftDeletable : class, ISoftDeletable {
            softDeletable.SetDeleted(isDeleted: false);
        }

        internal static void SetDeleted<TSoftDeletable>(this TSoftDeletable softDeletable, Boolean isDeleted) where TSoftDeletable : class, ISoftDeletable {
            SetterCache<TSoftDeletable>.DeletedPropertySetter(softDeletable, isDeleted ? DateTime.UtcNow : (DateTime?) null);
            //softDeletable.Deleted = isDeleted ? DateTime.UtcNow : (DateTime?)null;
        }
    }
}