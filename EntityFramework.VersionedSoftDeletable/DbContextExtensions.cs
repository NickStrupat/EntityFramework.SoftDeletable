using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using EntityFramework.Filters;

namespace EntityFramework.VersionedSoftDeletable {
	public static class DbContextExtensions {
		private const String VersionedSoftDeletableFilterName = "VersionedSoftDeletable";

		public static void EnableSoftDeletableFilter(this DbContext dbContext) {
			dbContext.EnableFilter(VersionedSoftDeletableFilterName);
		}

		public static void DisableSoftDeletableFilter(this DbContext dbContext) {
			dbContext.DisableFilter(VersionedSoftDeletableFilterName);
		}

		public static void AddSoftDeletableFilter(this ConventionsConfiguration conventions) {
			conventions.Add(FilterConvention.Create(VersionedSoftDeletableFilterName, VersionedSoftDeletable.IsNotDeletedExpression));
		}

		private static void CheckVersionedSoftDeletableType<TVersionedSoftDeletable>() {
			var type = typeof(TVersionedSoftDeletable);
			if (!typeof(IVersionedSoftDeletable).IsAssignableFrom(type) || !typeof(IVersionedUserSoftDeletable).IsAssignableFrom(type))
				throw new InvalidOperationException(String.Format("TVersionedSoftDeletable must implement {0} or {1}", typeof(IVersionedSoftDeletable).Name, typeof(IVersionedUserSoftDeletable).Name));
		}

		public static void EnableVersionedSoftDeletableFilter<TVersionedSoftDeletable>(this DbContext dbContext) {
			CheckVersionedSoftDeletableType<TVersionedSoftDeletable>();
			dbContext.EnableFilter(VersionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name);
		}

		public static void DisableVersionedSoftDeletableFilter<TVersionedSoftDeletable>(this DbContext dbContext) {
			CheckVersionedSoftDeletableType<TVersionedSoftDeletable>();
			dbContext.DisableFilter(VersionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name);
		}

		public static void AddSoftDeletableFilter<TVersionedSoftDeletable>(this ConventionsConfiguration conventions) {
			CheckVersionedSoftDeletableType<TVersionedSoftDeletable>();
			if (typeof(IVersionedSoftDeletable).IsAssignableFrom(typeof(TVersionedSoftDeletable)))
				conventions.Add(FilterConvention.Create(VersionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name, VersionedSoftDeletable.IsNotDeletedExpression));
			else if (typeof(IVersionedUserSoftDeletable).IsAssignableFrom(typeof(TVersionedSoftDeletable)))
				conventions.Add(FilterConvention.Create(VersionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name, VersionedUserSoftDeletable.IsNotDeletedExpression));
			else
				throw new InvalidOperationException("Unknown type: " + typeof(TVersionedSoftDeletable));
		}
	}
}
