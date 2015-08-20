using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedUserSoftDeletableExtensions {
		public static void InitializeVersionedUserSoftDeletable(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			versionedUserSoftDeletable.InitializeVersionedProperties();
			versionedUserSoftDeletable.Triggers().Deleting += e => {
				e.Entity.SoftDelete();
				e.Cancel();
			};
			versionedUserSoftDeletable.Triggers().Updating += e => {
				if (e.Entity.IsDeleted())
					throw new SoftDeletable.SoftDeletableModifiedWhileDeletedException();
			};
		}

		public static Boolean IsDeleted(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			return versionedUserSoftDeletable.Deleted.Value.IsDeleted;
		}

		public static void SoftDelete(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			versionedUserSoftDeletable.SetDeleted(newDeletedState: true);
		}

		public static void Restore(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			versionedUserSoftDeletable.SetDeleted(newDeletedState: false);
		}

		private static void SetDeleted(this IVersionedUserSoftDeletable versionedUserSoftDeletable, Boolean newDeletedState) => versionedUserSoftDeletable.Deleted.Value = new UserDeleted(versionedUserSoftDeletable.GetCurrentUserId(), newDeletedState);
	}
}
