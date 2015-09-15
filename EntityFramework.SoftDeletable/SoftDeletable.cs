using System;

using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
	public abstract class SoftDeletable : ISoftDeletable {
		public DateTime? Deleted { get; internal set; }

		public static void Initialize() { }

		static SoftDeletable() {
			Triggers<ISoftDeletable>.Deleting += entry => {
				entry.Entity.SoftDelete();
				entry.Cancel();
			};
			Triggers<ISoftDeletable>.Updating += entry => {
				var initialDeletedValue = GetInitialDeletedValue(entry);
				var currentDeletedValue = GetCurrentDeletedValue(entry);
				ValidateUpdatingSoftDeletable(initialDeletedValue, currentDeletedValue);
			};
		}

		private static DateTime? GetInitialDeletedValue(IBeforeEntry<ISoftDeletable> entry) {
			return (DateTime?) entry.Context.Entry(entry.Entity).OriginalValues[nameof(ISoftDeletable.Deleted)];
		}

		private static DateTime? GetCurrentDeletedValue(IBeforeEntry<ISoftDeletable> entry) {
			return entry.Entity.Deleted;
		}

		public static void ValidateUpdatingSoftDeletable(DateTime? initialDeletedValue, DateTime? currentDeletedValue) {
			var wasInitiallyDeleted = initialDeletedValue != null;
			var isCurrentlyDeleted = currentDeletedValue != null;
			if (wasInitiallyDeleted && isCurrentlyDeleted) {
				if (currentDeletedValue.Value < initialDeletedValue.Value)
					throw new SoftDeletableMarkedDeletedIncorrectly();
				throw new SoftDeletableModifiedWhileDeletedException();
			}
		}
	}
}