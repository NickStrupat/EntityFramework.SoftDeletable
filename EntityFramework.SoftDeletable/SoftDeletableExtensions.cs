using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
	public static class SoftDeletableExtensions {
        public static void InitializeSoftDeletable(this ISoftDeletable softDeletable) {
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

	    public static void Restore(this ISoftDeletable softDeletable) {
	        softDeletable.SetDeleted(isDeleted: false);
	    }

        internal static void SetDeleted(this ISoftDeletable softDeletable, Boolean isDeleted) {
            softDeletable.Deleted = isDeleted ? DateTime.UtcNow : (DateTime?)null;
		}
	}
}