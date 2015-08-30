using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public interface IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted/*, TUserDeleted, TUserDeletedVersion, TIUserDeleteds*/> : IVersionedProperties
		//where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
		//where TUserDeletedVersion : VersionBase<TUserDeleted>, new()
	{
		TVersionedUserDeleted Deleted { get; }
		TUserId GetCurrentUserId();
	}

	public interface IVersionedUserSoftDeletable : IVersionedUserSoftDeletable<String, VersionedUserDeleted/*, UserDeleted, UserDeletedVersion, IUserDeleteds*/> {}

	public abstract class UserDeleted<TUserId> {
		public TUserId UserId { get; private set; } // We're unable to apply a foreign constraint here due to current limitations of complex types in Entity Framework 6
		public Boolean IsDeleted { get; private set; }

		protected UserDeleted() {}
		protected UserDeleted(TUserId userId, Boolean isDeleted) {
			UserId = userId;
			IsDeleted = isDeleted;
		}
	}

	[ComplexType]
	public class VersionedUserDeleted : VersionedBase<UserDeleted, UserDeletedVersion, IUserDeleteds> {
		protected override UserDeleted DefaultValue => new UserDeleted(null, false);
		protected override Func<IUserDeleteds, DbSet<UserDeletedVersion>> VersionDbSet => x => x.UserDeleteds;
	}

	[ComplexType]
	public class UserDeleted : UserDeleted<String> {
		internal UserDeleted(String userId, Boolean isDeleted) : base(userId, isDeleted) {}
	}

	public class UserDeletedVersion : VersionBase<UserDeleted> {}

	public interface IUserDeleteds {
		DbSet<UserDeletedVersion> UserDeleteds { get; set; }
	}
}