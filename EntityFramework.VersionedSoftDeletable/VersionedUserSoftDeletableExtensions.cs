using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedUserSoftDeletableExtensions {
		public static void InitializeVersionedUserSoftDeletable(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			versionedUserSoftDeletable.InitializeVersionedProperties();
			versionedUserSoftDeletable.Triggers().Deleting += e => {
				e.Entity.SetDeleted(isDeleted: true);
				e.Cancel();
			};
			versionedUserSoftDeletable.Triggers().Updating += e => {
				if (e.Entity.IsDeleted())
					throw new SoftDeletable.SoftDeletableModifiedWhileDeletedException();
			};
		}

		public static Boolean IsDeleted(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			return versionedUserSoftDeletable.Deleted.Value.IsDeleted;
		}

		public static void SoftDelete(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			versionedUserSoftDeletable.SetDeleted(isDeleted: true);
		}

		public static void Restore(this IVersionedUserSoftDeletable versionedUserSoftDeletable) {
			versionedUserSoftDeletable.SetDeleted(isDeleted: false);
		}

		internal static void SetDeleted(this IVersionedUserSoftDeletable versionedUserSoftDeletable, Boolean isDeleted) {
			versionedUserSoftDeletable.Deleted.Value = new UserDeleted(versionedUserSoftDeletable.GetCurrentUserId(), isDeleted);
		}
	}
}
