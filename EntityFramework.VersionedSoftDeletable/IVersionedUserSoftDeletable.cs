using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public interface IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> : IVersionedProperties {
		TVersionedUserDeleted Deleted { get; }
		TUserId GetCurrentUserId();
	}

	public abstract class UserDeleted<TUserId> {
		public TUserId UserId { get; private set; } // We're unable to apply a foreign constraint here due to current limitations of complex types in Entity Framework 6
		public Boolean IsDeleted { get; private set; }

		protected UserDeleted() {}
		protected UserDeleted(TUserId userId, Boolean isDeleted) {
			if (userId == null)
				throw new ArgumentNullException(nameof(userId));
			UserId = userId;
			IsDeleted = isDeleted;
		}
	}

	public interface IVersionedUserSoftDeletable : IVersionedUserSoftDeletable<String, VersionedUserDeleted> {}

	[ComplexType]
	public class VersionedUserDeleted : VersionedBase<UserDeleted, UserDeletedVersion, IUserDeleteds> {
		protected override UserDeleted DefaultValue => new UserDeleted();
		protected override Func<IUserDeleteds, DbSet<UserDeletedVersion>> VersionDbSet => x => x.UserDeleteds;
	}

	[ComplexType]
	public class UserDeleted : UserDeleted<String> {
		internal UserDeleted() {}
		internal UserDeleted(String userId, Boolean isDeleted) : base(userId, isDeleted) {}
	}

	public class UserDeletedVersion : VersionBase<UserDeleted> { }

	public interface IUserDeleteds {
		DbSet<UserDeletedVersion> UserDeleteds { get; set; }
	}
}