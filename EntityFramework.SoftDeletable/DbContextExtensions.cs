using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using EntityFramework.Filters;

namespace EntityFramework.SoftDeletable {
    public static class DbContextExtensions {
        private const String SoftDeletableFilterName = "SoftDeletable";

        public static void EnableSoftDeletableFilter(this DbContext dbContext) {
            dbContext.EnableFilter(SoftDeletableFilterName);
        }

        public static void DisableSoftDeletableFilter(this DbContext dbContext) {
            dbContext.DisableFilter(SoftDeletableFilterName);
        }

        public static void AddSoftDeletableFilter(this ConventionsConfiguration conventions) {
            conventions.Add(FilterConvention.Create(SoftDeletableFilterName, SoftDeletable.IsNotDeletedExpression));
        }

        public static void EnableSoftDeletableFilter<TSoftDeletable>(this DbContext dbContext) where TSoftDeletable : ISoftDeletable {
            dbContext.EnableFilter(SoftDeletableFilterName + typeof(TSoftDeletable).Name);
        }

        public static void DisableSoftDeletableFilter<TSoftDeletable>(this DbContext dbContext) where TSoftDeletable : ISoftDeletable {
            dbContext.DisableFilter(SoftDeletableFilterName + typeof(TSoftDeletable).Name);
        }

        public static void AddSoftDeletableFilter<TSoftDeletable>(this ConventionsConfiguration conventions) where TSoftDeletable : ISoftDeletable {
            conventions.Add(FilterConvention.Create(SoftDeletableFilterName + typeof(TSoftDeletable).Name, SoftDeletable.IsNotDeletedExpression));
        }
    }
}