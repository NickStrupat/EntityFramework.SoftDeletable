using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
	public static class SoftDeletableExtensions {
		private class Setters {
			public readonly Action<ISoftDeletable, DateTime?> DeletedPropertySetter;
			public readonly Action<ISoftDeletable> DeletedChangedByIdPropertySetter;

			public Setters(Type softDeletableType) {
				DeletedPropertySetter = PropertyReflection.GetValueSetter<ISoftDeletable, DateTime?>(softDeletableType.GetProperty(nameof(ISoftDeletable.Deleted)));
				DeletedChangedByIdPropertySetter = GetDeletedChangedByIdPropertySetter(softDeletableType);
			}

			private static Action<ISoftDeletable> GetDeletedChangedByIdPropertySetter(Type softDeletableType) {
				var @interface = softDeletableType.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IUserSoftDeletable<>));
				if (@interface == null)
					return null;

				var userIdType = @interface.GenericTypeArguments.Single();
				var setDeletedChangedByIdMethod = typeof(SoftDeletableExtensions).GetMethod(nameof(SetDeletedChangedById), BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(softDeletableType, userIdType);

				var instance = Expression.Parameter(typeof(ISoftDeletable));
				var softDeletable = Expression.Convert(instance, softDeletableType);
				var setterCall = Expression.Call(setDeletedChangedByIdMethod, softDeletable);
				return Expression.Lambda<Action<ISoftDeletable>>(setterCall, instance).Compile();
			}
		}

		private static readonly ConcurrentDictionary<Type, Setters> setterCache = new ConcurrentDictionary<Type, Setters>();

		private static Setters GetSetters(this ISoftDeletable softDeletable) {
			return setterCache.GetOrAdd(softDeletable.GetType(), t => new Setters(t));
		}

		private static class SetterCache<TUserSoftDeletable, TUserId> where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
			public static readonly Action<TUserSoftDeletable, TUserId> DeletedChangedByIdPropertySetter = PropertyReflection.GetValueSetter<TUserSoftDeletable, TUserId>(
				typeof(TUserSoftDeletable).GetProperty(nameof(IUserSoftDeletable<TUserId>.DeletedChangedById))
			);
		}

		private static void SetDeletedChangedById<TUserSoftDeletable, TUserId>(this TUserSoftDeletable userSoftDeletable) where TUserSoftDeletable : class, IUserSoftDeletable<TUserId> {
			SetterCache<TUserSoftDeletable, TUserId>.DeletedChangedByIdPropertySetter(userSoftDeletable, userSoftDeletable.GetCurrentUserId());
		}

		public static Boolean IsDeleted(this ISoftDeletable softDeletable) {
			if (softDeletable == null)
				throw new ArgumentNullException(nameof(softDeletable));
			return softDeletable.Deleted != null;
		}

		public static void SoftDelete(this ISoftDeletable softDeletable) {
			if (softDeletable == null)
				throw new ArgumentNullException(nameof(softDeletable));
			if (!softDeletable.IsDeleted())
				softDeletable.SetSoftDeletable(markAsDeleted: true);
		}

		public static void Restore(this ISoftDeletable softDeletable) {
			if (softDeletable == null)
				throw new ArgumentNullException(nameof(softDeletable));
			if (softDeletable.IsDeleted())
				softDeletable.SetSoftDeletable(markAsDeleted: false);
		}

		private static void SetSoftDeletable(this ISoftDeletable softDeletable, Boolean markAsDeleted) {
			var setters = softDeletable.GetSetters();
			setters.DeletedPropertySetter(softDeletable, markAsDeleted ? DateTime.UtcNow : (DateTime?)null);
			setters.DeletedChangedByIdPropertySetter?.Invoke(softDeletable);
		}
	}
};