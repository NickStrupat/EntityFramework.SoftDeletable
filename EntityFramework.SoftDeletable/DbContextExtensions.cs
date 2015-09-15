using System;
using System.Data.Entity;
using EntityFramework.DynamicFilters;

namespace EntityFramework.SoftDeletable {
	public static class DbContextExtensions {
		private const String filterName = "SoftDeletable";

		public static void AddSoftDeletableFilter(this DbModelBuilder mb) {
			mb.Filter(filterName, FilterExpression<ISoftDeletable>.IsNotDeleted);
		}
		public static void EnableSoftDeletableFilter(this DbContext dbc) {
			dbc.EnableFilter(filterName);
		}
		public static void DisableSoftDeletableFilter(this DbContext dbc) {
			dbc.DisableFilter(filterName);
		}

		public static void AddSoftDeletableFilter<T>(this DbModelBuilder mb) where T : ISoftDeletable {
			mb.Filter(filterName + typeof (T).Name, FilterExpression<T>.IsNotDeleted);
		}
		public static void EnableSoftDeletableFilter<T>(this DbContext dbc) where T : ISoftDeletable {
			dbc.EnableFilter(filterName + typeof (T).Name);
		}
		public static void DisableSoftDeletableFilter<T>(this DbContext dbc) where T : ISoftDeletable {
			dbc.DisableFilter(filterName + typeof (T).Name);
		}
	}
}