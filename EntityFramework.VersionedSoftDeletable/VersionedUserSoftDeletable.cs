using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq.Expressions;

using EntityFramework.VersionedProperties;
using EntityFramework.Triggers;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedUserSoftDeletable : VersionedUserSoftDeletable<String, VersionedUserDeleted>, IVersionedUserSoftDeletable {
		//internal new static Expression<Func<IVersionedUserSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value.IsDeleted;
	}

	public abstract class VersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> : IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> {
		public TVersionedUserDeleted Deleted { get; internal set; }
		public abstract TUserId GetCurrentUserId();

		protected VersionedUserSoftDeletable() {
			this.InitializeVersionedUserSoftDeletable();
		}

		public static void Initialize() { }

		static VersionedUserSoftDeletable() {
			Triggers<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>.Deleting += entry => {
				entry.Entity.SoftDelete();
				entry.Cancel();
			};
			Triggers<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>.Updating += entry => {
				var initialDeletedValue = GetInitialDeletedValue(entry);
				var currentDeletedValue = GetCurrentDeletedValue(entry);
				SoftDeletable.SoftDeletable.ValidateUpdatingSoftDeletable(initialDeletedValue, currentDeletedValue);
			};
		}

		private static DateTime? GetInitialDeletedValue(IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>> entry) {
			return LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>, TUserId, TVersionedUserDeleted>.InitialDeletedValue(entry);
		}

		private static DateTime? GetCurrentDeletedValue(IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>> entry) {
			return LateBoundMethods<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>, TUserId, TVersionedUserDeleted>.CurrentDeletedValue(entry);
		}
	}

	[ComplexType]
	public class VersionedUserDeleted : VersionedBase<UserDeleted, UserDeletedVersion, IUserDeleteds> {
		protected override UserDeleted DefaultValue => new UserDeleted(null, false);
		protected override Func<IUserDeleteds, DbSet<UserDeletedVersion>> VersionDbSet => x => x.UserDeleteds;
	}

	public abstract class UserDeleted<TUserId> {
		public TUserId UserId { get; private set; } // We're unable to apply a foreign constraint here due to current limitations of complex types in Entity Framework 6
		public Boolean IsDeleted { get; private set; }

		protected UserDeleted() { }
		protected UserDeleted(TUserId userId, Boolean isDeleted) {
			UserId = userId;
			IsDeleted = isDeleted;
		}
	}

	[ComplexType]
	public class UserDeleted : UserDeleted<String> {
		internal UserDeleted(String userId, Boolean isDeleted) : base(userId, isDeleted) { }
	}

	public class UserDeletedVersion : VersionBase<UserDeleted> { }

	public interface IUserDeleteds {
		DbSet<UserDeletedVersion> UserDeleteds { get; set; }
	}
}