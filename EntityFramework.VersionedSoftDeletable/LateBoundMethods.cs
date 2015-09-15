using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using EntityFramework.Triggers;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	internal static class LateBoundMethods<TIVersionedUserSoftDeletable, TUserId, TVersionedUserDeleted> where TIVersionedUserSoftDeletable : IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> {
		private static readonly Type thisType = typeof(LateBoundMethods<TIVersionedUserSoftDeletable, TUserId, TVersionedUserDeleted>);
		#region IsDeleted
		public static readonly Func<TIVersionedUserSoftDeletable, Boolean> IsDeleted = GetIsDeletedFunc();

		private static Boolean isDeleted<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> vusd)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new() {
			return vusd.Deleted.Value.IsDeleted;
		}

		private static Func<TIVersionedUserSoftDeletable, Boolean> GetIsDeletedFunc() {
			var types = new VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable>();
			var mi = thisType.GetMethod(nameof(isDeleted), BindingFlags.NonPublic | BindingFlags.Static);
			var gmi = mi.MakeGenericMethod(types.UserIdType, types.VersionedUserDeletedType, types.UserDeletedType, types.UserDeletedVersionType, types.UserDeletedsInterfaceType);
			var parameter = Expression.Parameter(typeof (IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>));
			return Expression.Lambda<Func<TIVersionedUserSoftDeletable, Boolean>>(Expression.Call(gmi, parameter), parameter).Compile();
		}
		#endregion
		
		#region SetDeleted
		public static readonly Action<TIVersionedUserSoftDeletable, Boolean> SetDeleted = GetSetDeletedAction();

		private static void setDeleted<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> vusd, Boolean markAsDeleted)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new() {
			vusd.Deleted.Value = UserDeletedFactories<TUserDeleted, TUserId>.Create(vusd.GetCurrentUserId(), markAsDeleted);
		}

		private static Action<TIVersionedUserSoftDeletable, Boolean> GetSetDeletedAction() {
			var types = new VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable>();
			var mi = thisType.GetMethod(nameof(setDeleted), BindingFlags.NonPublic | BindingFlags.Static);
			var gmi = mi.MakeGenericMethod(types.UserIdType, types.VersionedUserDeletedType, types.UserDeletedType, types.UserDeletedVersionType, types.UserDeletedsInterfaceType);
			var vusd = Expression.Parameter(typeof(IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>));
			var markAsDeleted = Expression.Parameter(typeof (Boolean));
			return Expression.Lambda<Action<TIVersionedUserSoftDeletable, Boolean>>(Expression.Call(gmi, vusd, markAsDeleted), vusd, markAsDeleted).Compile();
		}

		private static class UserDeletedFactories<TUserDeleted, TUserId> where TUserDeleted : UserDeleted<TUserId> {
			public static readonly Func<TUserId, Boolean, TUserDeleted> Create = GetConstructor();

			private static Func<TUserId, Boolean, TUserDeleted> GetConstructor() {
				var parameterTypes = new[] {typeof (TUserId), typeof (Boolean)};
				var dynamicMethod = new DynamicMethod(typeof (TUserDeleted).Name + "Ctor", typeof (TUserDeleted), parameterTypes, typeof (TUserDeleted), true);
				var ilGen = dynamicMethod.GetILGenerator();
				ilGen.Emit(OpCodes.Ldarg_0);
				ilGen.Emit(OpCodes.Ldarg_1);
				ilGen.Emit(OpCodes.Newobj, typeof (TUserDeleted).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, new ParameterModifier[0]));
				ilGen.Emit(OpCodes.Ret);
				return (Func<TUserId, Boolean, TUserDeleted>) dynamicMethod.CreateDelegate(typeof (Func<TUserId, Boolean, TUserDeleted>));
			}
		}
		#endregion

		public static readonly Func<IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>, DateTime?> InitialDeletedValue = GetInitialDeletedValueFunc(nameof(initialDeletedValue));

		private static DateTime? initialDeletedValue<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>> entry)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new()
		{
			var earliestLocalVersion = entry.Entity.Deleted.LocalVersions.FirstOrDefault();
			if (earliestLocalVersion != null)
				return earliestLocalVersion.Value.IsDeleted ? earliestLocalVersion.Added : (DateTime?) null;
			return entry.Entity.Deleted.Value.IsDeleted ? entry.Entity.Deleted.Modified : (DateTime?) null;
		}

		public static readonly Func<IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>, DateTime?> CurrentDeletedValue = GetInitialDeletedValueFunc(nameof(currentDeletedValue));

		private static DateTime? currentDeletedValue<TUserId, TVersionedUserDeleted, TUserDeleted, TUserDeletedVersion, TIUserDeleteds>(IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>> entry)
			where TVersionedUserDeleted : VersionedBase<TUserDeleted, TUserDeletedVersion, TIUserDeleteds>
			where TUserDeleted : UserDeleted<TUserId>
			where TUserDeletedVersion : VersionBase<TUserDeleted>, new() {
			return entry.Entity.Deleted.Value.IsDeleted ? entry.Entity.Deleted.Modified : (DateTime?) null;
		}

		private static Func<IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>, DateTime?> GetInitialDeletedValueFunc(String deletedValueMethodName) {
			var types = new VersionedUserSoftDeletableInnerTypes<TIVersionedUserSoftDeletable>();
			var mi = thisType.GetMethod(deletedValueMethodName, BindingFlags.NonPublic | BindingFlags.Static);
			var gmi = mi.MakeGenericMethod(types.UserIdType, types.VersionedUserDeletedType, types.UserDeletedType, types.UserDeletedVersionType, types.UserDeletedsInterfaceType);
			var parameter = Expression.Parameter(typeof(IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>));
			return Expression.Lambda<Func<IBeforeEntry<IVersionedUserSoftDeletable<TUserId, TVersionedUserDeleted>>, DateTime?>>(Expression.Call(gmi, parameter), parameter).Compile();
		}
	}
}
