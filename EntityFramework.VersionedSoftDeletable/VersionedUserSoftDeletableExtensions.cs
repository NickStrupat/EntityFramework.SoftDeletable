using System;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public static class VersionedUserSoftDeletableExtensions {
		public static void InitializeVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			versionedUserSoftDeletable.InitializeVersionedProperties();
		}

		public static Boolean IsDeleted<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			return LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>, TUserId, TVersionedUserDeleted>.IsDeleted(versionedUserSoftDeletable);
		}

		public static void SoftDelete<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			if (!versionedUserSoftDeletable.IsDeleted())
				versionedUserSoftDeletable.SetDeleted(markAsDeleted: true);
		}

		public static void Restore<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable) {
			if (versionedUserSoftDeletable == null)
				throw new ArgumentNullException(nameof(versionedUserSoftDeletable));
			if (versionedUserSoftDeletable.IsDeleted())
				versionedUserSoftDeletable.SetDeleted(markAsDeleted: false);
		}

		private static void SetDeleted<TUserId, TVersionedUserDeleted>(this IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable, Boolean markAsDeleted) {
			LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>, TUserId, TVersionedUserDeleted>.SetDeleted(versionedUserSoftDeletable, markAsDeleted);
		}
	}
}
