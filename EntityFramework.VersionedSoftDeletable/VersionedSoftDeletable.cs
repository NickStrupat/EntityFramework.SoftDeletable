using System;
using System.Linq;

using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedSoftDeletable : IVersionedSoftDeletable {
		public VersionedBoolean Deleted { get; internal set; }

		protected VersionedSoftDeletable() {
			this.InitializeVersionedSoftDeletable();
		}

		public static void Initialize() { }

		static VersionedSoftDeletable() {
			Triggers<IVersionedSoftDeletable>.Deleting += entry => {
				entry.Entity.SoftDelete();
				entry.Cancel();
			};
			Triggers<IVersionedSoftDeletable>.Updating += entry => {
				var initialDeletedValue = GetInitialDeletedValue(entry);
				var currentDeletedValue = GetCurrentDeletedValue(entry);
				SoftDeletable.SoftDeletable.ValidateUpdatingSoftDeletable(initialDeletedValue, currentDeletedValue);
			};
		}

		private static DateTime? GetInitialDeletedValue(IBeforeEntry<IVersionedSoftDeletable> entry) {
			var earliestLocalVersion = entry.Entity.Deleted.LocalVersions.FirstOrDefault();
			if (earliestLocalVersion != null)
				return earliestLocalVersion.Value ? earliestLocalVersion.Added : (DateTime?) null;
			return entry.Entity.Deleted.Value ? entry.Entity.Deleted.Modified : (DateTime?) null;
		}

		private static DateTime? GetCurrentDeletedValue(IBeforeEntry<IVersionedSoftDeletable> entry) {
			return entry.Entity.Deleted.Value ? entry.Entity.Deleted.Modified : (DateTime?) null;
		}
	}
}