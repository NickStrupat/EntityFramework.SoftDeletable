using System;
using System.Linq.Expressions;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public abstract class VersionedUserSoftDeletable : VersionedUserSoftDeletable<String, VersionedUserDeleted/*, UserDeleted, UserDeletedVersion, IUserDeleteds*/>, IVersionedUserSoftDeletable {
		internal new static Expression<Func<IVersionedUserSoftDeletable, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value.IsDeleted;
	}

	public abstract class VersionedUserSoftDeletable<TUserId, TVersionedUserDeleted/*, TUserDeleted, TUserDeletedVersion, TIUserDeleteds*/>
		: IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted/*, TUserDeleted, TUserDeletedVersion, TIUserDeleteds*/>
		//where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
		//where TUserDeleted : UserDeleted<TUserId>
		//where TUserDeletedVersion : VersionBase<TUserDeleted>, new() 
	{
		public TVersionedUserDeleted Deleted { get; internal set; }
		public abstract TUserId GetCurrentUserId();

		protected VersionedUserSoftDeletable() {
			this.InitializeVersionedUserSoftDeletable();
		}

		//internal static Expression<Func<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted/*, TUserDeleted, TUserDeletedVersion, TIUserDeleteds*/>, Boolean>> IsNotDeletedExpression = x => !x.Deleted.Value.IsDeleted;
	}
}