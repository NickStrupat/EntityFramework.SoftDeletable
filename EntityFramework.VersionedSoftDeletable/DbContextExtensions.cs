using System;
using System.Data.Entity;
using EntityFramework.DynamicFilters;

namespace EntityFramework.VersionedSoftDeletable {
	public static class DbContextExtensions {
		#region IVersionedSoftDeletable
		private const String versionedSoftDeletableFilterName = "VersionedSoftDeletable";

		public static void AddVersionedSoftDeletableFilter(this DbModelBuilder modelBuilder) {
			modelBuilder.Filter(versionedSoftDeletableFilterName, FilterExpression<IVersionedSoftDeletable>.IsNotDeletedExpression);
		}

		public static void EnableVersionedSoftDeletableFilter(this DbContext dbContext) {
			dbContext.EnableFilter(versionedSoftDeletableFilterName);
		}

		public static void DisableVersionedSoftDeletableFilter(this DbContext dbContext) {
			dbContext.DisableFilter(versionedSoftDeletableFilterName);
		}

		public static void AddVersionedSoftDeletableFilter<TVersionedSoftDeletable>(this DbModelBuilder modelBuilder) where TVersionedSoftDeletable : IVersionedSoftDeletable {
			modelBuilder.Filter(versionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name, FilterExpression<TVersionedSoftDeletable>.IsNotDeletedExpression);
		}

		public static void EnableVersionedSoftDeletableFilter<TVersionedSoftDeletable>(this DbContext dbContext) where TVersionedSoftDeletable : IVersionedSoftDeletable {
			dbContext.EnableFilter(versionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name);
		}

		public static void DisableVersionedSoftDeletableFilter<TVersionedSoftDeletable>(this DbContext dbContext) where TVersionedSoftDeletable : IVersionedSoftDeletable {
			dbContext.DisableFilter(versionedSoftDeletableFilterName + typeof(TVersionedSoftDeletable).Name);
		}
		#endregion
		#region IVersionedUserSoftDeletable
		private const String versionedUserSoftDeletableFilterName = "VersionedUserSoftDeletable";

		public static void AddVersionedUserSoftDeletableFilter(this DbModelBuilder modelBuilder) {
			modelBuilder.Filter(versionedUserSoftDeletableFilterName, FilterExpression<IVersionedUserSoftDeletable<String, UserDeleted>, String, UserDeleted>.IsNotDeletedExpression);
		}

		public static void EnableVersionedUserSoftDeletableFilter(this DbContext dbContext) {
			dbContext.EnableFilter(versionedUserSoftDeletableFilterName);
		}

		public static void DisableVersionedUserSoftDeletableFilter(this DbContext dbContext) {
			dbContext.DisableFilter(versionedUserSoftDeletableFilterName);
		}
		#endregion
	}
}
