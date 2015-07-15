using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public interface IVersionedUserSoftDeletable : IVersionedProperties {
		VersionedUserDeleted Deleted { get; }
		String GetCurrentUserId();
	}

	[ComplexType]
	public class VersionedUserDeleted : VersionedBase<UserDeleted, UserDeletedVersion, IUserDeleteds> {
		protected override UserDeleted DefaultValue { get { return new UserDeleted(); } }
		protected override Func<IUserDeleteds, DbSet<UserDeletedVersion>> VersionDbSet {
			get { return x => x.UserDeleteds; }
		}
	}

	[ComplexType]
	public class UserDeleted {
		public String UserId { get; protected set; } // We're unable to apply a foreign constraint here due to current limitations of complex types in Entity Framework 6

		public Boolean IsDeleted { get; protected set; }

		internal UserDeleted() { }

		internal UserDeleted(String userId, Boolean isDeleted) {
			if (userId == null)
				throw new ArgumentNullException("userId");
			UserId = userId;
			IsDeleted = isDeleted;
		}
	}

	public class UserDeletedVersion : VersionBase<UserDeleted> { }

	public interface IUserDeleteds {
		DbSet<UserDeletedVersion> UserDeleteds { get; set; }
	}
}