using System;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	internal class VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable> {
		public VersionedUserSoftDeletableInnerTypes() {
			var genericArgs = typeof(TIVersionedUserSoftDeletable).GenericTypeArguments;
			UserIdType = genericArgs[0];
			VersionedUserDeletedType = genericArgs[1];
			var versionedGenericArgs = VersionedUserDeletedType.BaseType.GenericTypeArguments;
			UserDeletedType = versionedGenericArgs[0];
			UserDeletedVersionType = versionedGenericArgs[1];
			UserDeletedsInterfaceType = versionedGenericArgs[2];

			VersionedBaseType = typeof (VersionedBase<,,>).MakeGenericType(UserDeletedType, UserDeletedVersionType, UserDeletedsInterfaceType);
			if (!VersionedBaseType.IsAssignableFrom(VersionedUserDeletedType))
				throw new InvalidOperationException();

			var userDeletedBaseType = typeof (UserDeleted<>).MakeGenericType(UserIdType);
			if (!userDeletedBaseType.IsAssignableFrom(UserDeletedType))
				throw new InvalidOperationException();

			var versionBaseType = typeof (VersionBase<>).MakeGenericType(UserDeletedType);
			if (!versionBaseType.IsAssignableFrom(UserDeletedVersionType))
				throw new InvalidOperationException();
		}

		public Type VersionedBaseType { get; }
		public Type UserIdType { get; }
		public Type VersionedUserDeletedType { get; }
		public Type UserDeletedType { get; }
		public Type UserDeletedVersionType { get; }
		public Type UserDeletedsInterfaceType { get; }
	}
}