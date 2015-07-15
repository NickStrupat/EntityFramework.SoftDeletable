using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedSoftDeletableExtensions {
		public static void InitializeVersionedSoftDeletable(this IVersionedSoftDeletable versionedSoftDeletable) {
			versionedSoftDeletable.InitializeVersionedProperties();
			versionedSoftDeletable.Triggers().Deleting += e => {
				e.Entity.SetDeleted(isDeleted: true);
				e.Cancel();
			};
			versionedSoftDeletable.Triggers().Updating += e => {
				if (e.Entity.IsDeleted())
					throw new SoftDeletable.SoftDeletableModifiedWhileDeletedException();
			};
		}

		public static Boolean IsDeleted(this IVersionedSoftDeletable versionedSoftDeletable) {
			return versionedSoftDeletable.Deleted.Value;
		}

		public static void SoftDelete(this IVersionedSoftDeletable versionedSoftDeletable) {
			versionedSoftDeletable.SetDeleted(isDeleted: true);
		}

		public static void Restore(this IVersionedSoftDeletable versionedSoftDeletable) {
			versionedSoftDeletable.SetDeleted(isDeleted: false);
		}

		internal static void SetDeleted(this IVersionedSoftDeletable versionedSoftDeletable, Boolean isDeleted) {
			versionedSoftDeletable.Deleted.Value = isDeleted;
		}
	}
}
