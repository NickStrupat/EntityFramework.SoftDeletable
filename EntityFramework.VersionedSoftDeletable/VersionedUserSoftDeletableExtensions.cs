using System;
using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedUserSoftDeletableExtensions {
		public static void InitializeVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
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

		public static Boolean IsDeleted<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			return LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>.IsDeleted(versionedUserSoftDeletable);
		}

		public static void SoftDelete<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			versionedUserSoftDeletable.SetDeleted(newDeletedState: true);
		}

		public static void Restore<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			versionedUserSoftDeletable.SetDeleted(newDeletedState: false);
		}

		private static void SetDeleted<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable, Boolean newDeletedState) {
			LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>.SetDeleted(versionedUserSoftDeletable, newDeletedState);
		}
	}
}
