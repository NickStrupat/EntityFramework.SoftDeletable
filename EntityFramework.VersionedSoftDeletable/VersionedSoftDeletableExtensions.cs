using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedSoftDeletableExtensions {
		public static void InitializeVersionedSoftDeletable(this IVersionedSoftDeletable versionedSoftDeletable) {
			versionedSoftDeletable.InitializeVersionedProperties();
			versionedSoftDeletable.Restore();
			versionedSoftDeletable.Triggers().Deleting += e => {
				e.Entity.SoftDelete();
				e.Cancel();
			};
			versionedSoftDeletable.Triggers().Updating += e => {
				if (e.Entity.IsDeleted())
					throw new SoftDeletable.SoftDeletableModifiedWhileDeletedException();
			};
		}

		public static Boolean IsDeleted(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			return versionedSoftDeletable.Deleted.Value;
		}

		public static void SoftDelete(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			versionedSoftDeletable.SetDeleted(newDeletedState: true);
		}

		public static void Restore(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			versionedSoftDeletable.SetDeleted(newDeletedState: false);
		}

		private static void SetDeleted(this IVersionedSoftDeletable versionedSoftDeletable, Boolean newDeletedState) => versionedSoftDeletable.Deleted.Value = newDeletedState;
	}
}
