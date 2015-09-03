using System;
using System.Data.Entity;
using EntityFramework.DynamicFilters;

namespace EntityFramework.SoftDeletable {
	public static class DbContextExtensions {
		private const String softDeletableFilterName = "SoftDeletable";

		public static void AddSoftDeletableFilter(this DbModelBuilder modelBuilder) {
			modelBuilder.Filter(softDeletableFilterName, FilterExpression<ISoftDeletable>.IsNotDeletedExpression);
		}

		public static void EnableSoftDeletableFilter(this DbContext dbContext) {
			dbContext.EnableFilter(softDeletableFilterName);
		}

		public static void DisableSoftDeletableFilter(this DbContext dbContext) {
			dbContext.DisableFilter(softDeletableFilterName);
		}

		public static void AddSoftDeletableFilter<TSoftDeletable>(this DbModelBuilder modelBuilder) where TSoftDeletable : ISoftDeletable {
			modelBuilder.Filter(softDeletableFilterName + typeof(TSoftDeletable).Name, FilterExpression<TSoftDeletable>.IsNotDeletedExpression);
		}

		public static void EnableSoftDeletableFilter<TSoftDeletable>(this DbContext dbContext) where TSoftDeletable : ISoftDeletable {
			dbContext.EnableFilter(softDeletableFilterName + typeof(TSoftDeletable).Name);
		}

		public static void DisableSoftDeletableFilter<TSoftDeletable>(this DbContext dbContext) where TSoftDeletable : ISoftDeletable {
			dbContext.DisableFilter(softDeletableFilterName + typeof(TSoftDeletable).Name);
		}
	}
}