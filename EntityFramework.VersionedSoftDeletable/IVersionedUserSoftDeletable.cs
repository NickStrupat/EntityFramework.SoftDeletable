using System;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public interface IVersionedUserSoftDeletable<out TUserId, out TVersionedUserDeleted> : IVersionedProperties {
		TVersionedUserDeleted Deleted { get; }
		TUserId GetCurrentUserId();
	}

	public interface IVersionedUserSoftDeletable : IVersionedUserSoftDeletable<String, VersionedUserDeleted> { }
}