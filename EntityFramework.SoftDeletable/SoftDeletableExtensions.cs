using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
	public static class SoftDeletableExtensions {
		internal const String DeletedPropertyName = "Deleted";

        internal static class SetterCache<TSoftDeletable> where TSoftDeletable : class, ISoftDeletable {
			public static readonly Action<TSoftDeletable, DateTime?> DeletedPropertySetter = PropertyReflection.GetValueSetter<TSoftDeletable, DateTime?>(typeof(TSoftDeletable).GetProperty(DeletedPropertyName));
        }

        public static void InitializeSoftDeletable<TSoftDeletable>(this TSoftDeletable softDeletable) where TSoftDeletable : class, ISoftDeletable {
            softDeletable.Triggers().Deleting += e => {
                e.Entity.SetDeleted(isDeleted: true);
                e.Cancel();
            };
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
        }
    }
}