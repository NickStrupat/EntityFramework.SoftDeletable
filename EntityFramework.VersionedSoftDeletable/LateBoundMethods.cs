using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	internal static class LateBoundMethods<TIVersionedUserSoftDeletable> {
		public static readonly Func<TIVersionedUserSoftDeletable, Boolean> IsDeleted = GetIsDeletedFunc();

		private static Func<TIVersionedUserSoftDeletable, Boolean> GetIsDeletedFunc() {
			var a = typeof(LateBoundMethods<TIVersionedUserSoftDeletable>).GetMethod(nameof(isDeleted), BindingFlags.NonPublic | BindingFlags.Static);
			var types = new VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable>();
			var b = a.MakeGenericMethod(types.UserIdType, types.VersionedUserDeletedType, types.UserDeletedType, types.UserDeletedVersionType, types.UserDeletedsInterfaceType);
			var parameter = Expression.Parameter(typeof(IVersionedUserSoftDeletable<,>).MakeGenericType(types.UserIdType, types.VersionedUserDeletedType));
			return Expression.Lambda<Func<TIVersionedUserSoftDeletable, Boolean>>(Expression.Call(b, parameter), parameter).Compile();
		}

		private static Boolean isDeleted<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new() {
			return versionedUserSoftDeletable.Deleted.Value.IsDeleted;
		}

		public static readonly Action<TIVersionedUserSoftDeletable, Boolean> SetDeleted = GetSetDeletedAction();

		private static Action<TIVersionedUserSoftDeletable, Boolean> GetSetDeletedAction() {
			var a = typeof(LateBoundMethods<TIVersionedUserSoftDeletable>).GetMethod(nameof(setDeleted), BindingFlags.NonPublic | BindingFlags.Static);
			var types = new VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable>();
			var b = a.MakeGenericMethod(types.UserIdType, types.VersionedUserDeletedType, types.UserDeletedType, types.UserDeletedVersionType, types.UserDeletedsInterfaceType);
			var parameter = Expression.Parameter(typeof(IVersionedUserSoftDeletable<,>).MakeGenericType(types.UserIdType, types.VersionedUserDeletedType));
			var parameter2 = Expression.Parameter(typeof(Boolean));
			return Expression.Lambda<Action<TIVersionedUserSoftDeletable, Boolean>>(Expression.Call(b, parameter, parameter2), parameter, parameter2).Compile();
		}

		private static void setDeleted<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> versionedUserSoftDeletable, Boolean newDeletedState)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new() {
			versionedUserSoftDeletable.Deleted.Value = UserDeletedFactories<TUserDeleted, TUserId>.Create(versionedUserSoftDeletable.GetCurrentUserId(), newDeletedState);
		}

		private static class UserDeletedFactories<TUserDeleted, TUserId> where TUserDeleted : UserDeleted<TUserId> {
			public static readonly Func<TUserId, Boolean, TUserDeleted> Create = GetConstructor();

			private static Func<TUserId, Boolean, TUserDeleted> GetConstructor() {
				var parameterTypes = new[] { typeof(TUserId), typeof(Boolean) };
				var dynamicMethod = new DynamicMethod(typeof(TUserDeleted).Name + "Ctor", typeof(TUserDeleted), parameterTypes, typeof(TUserDeleted), true);
				var ilGen = dynamicMethod.GetILGenerator();
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Ldarg_1);
				ilGen.Emit(OpCodes.Newobj, typeof(TUserDeleted).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, new ParameterModifier[0]));
				ilGen.Emit(OpCodes.Ret);
				return (Func<TUserId, Boolean, TUserDeleted>)dynamicMethod.CreateDelegate(typeof(Func<TUserId, Boolean, TUserDeleted>));
			}
		}
	}
}
