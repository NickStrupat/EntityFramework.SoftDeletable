using System;

using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedSoftDeletableExtensions {
		public static void InitializeVersionedSoftDeletable(this IVersionedSoftDeletable versionedSoftDeletable) {
			versionedSoftDeletable.InitializeVersionedProperties();
		}

		public static Boolean IsDeleted(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			return versionedSoftDeletable.Deleted.Value;
		}

		public static void SoftDelete(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			if (!versionedSoftDeletable.IsDeleted())
				versionedSoftDeletable.SetDeleted(markAsDeleted: true);
		}

		public static void Restore(this IVersionedSoftDeletable versionedSoftDeletable) {
			if (versionedSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedSoftDeletable));
			if (versionedSoftDeletable.IsDeleted())
				versionedSoftDeletable.SetDeleted(markAsDeleted: false);
		}

		private static void SetDeleted(this IVersionedSoftDeletable versionedSoftDeletable, Boolean markAsDeleted) {
			versionedSoftDeletable.Deleted.Value = markAsDeleted;
		}
	}
}
