using System;
using System.Data.Entity;

using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable
{
    public interface IVersionedSoftDeletable : IVersionedProperties
    {
        VersionedBoolean Deleted { get; }
    }

    public interface IVersionedUserSoftDeletable<TVersionedUserId, TUserId, TUserIdVersion, TIUserIdVersions> : IVersionedProperties
        where TVersionedUserId : VersionedBase<TUserId, TUserIdVersion, TIUserIdVersions>
        where TUserIdVersion : VersionBase<TUserId>, new()
    {
        TVersionedUserId Deleted { get; }
    }

    public class Wat<TUserId>
    {
        public class Bar : VersionBase<TUserId> {}

        public class Foo : VersionedBase<TUserId, Bar, IWooVersions> {
            protected override Func<IWooVersions, DbSet<Bar>> VersionDbSet
            {
                get { return x => x.WatVersions; }
            }
        }
        public interface IWooVersions { DbSet<Bar> WatVersions { get; set; } }
    }

    public abstract class VersionedUserSoftDeletable<TUserId> : IVersionedUserSoftDeletable<Wat<TUserId>.Foo, TUserId, Wat<TUserId>.Bar, Wat<TUserId>.IWooVersions>
    {
        public Wat<TUserId>.Foo Deleted { get; private set; }
    }
}
