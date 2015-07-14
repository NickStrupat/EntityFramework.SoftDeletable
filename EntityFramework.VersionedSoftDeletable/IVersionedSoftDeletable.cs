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

    internal class Wat<TUserId>
    {
        internal class Bar : VersionBase<TUserId> {}
        internal class Foo : VersionedBase<TUserId, Bar, IWooVersions> {
            protected override Func<IWooVersions, DbSet<Bar>> VersionDbSet
            {
                get { return x => x.WatVersions; }
            }
        }
        internal interface IWooVersions { DbSet<Bar> WatVersions { get; set; } }
    }

    public abstract class VersionedUserSoftDeletable<TUserId> : IVersionedUserSoftDeletable<Wat<TUserId>.Foo, TUserId, Wat<TUserId>.Bar, Wat<TUserId>.IWooVersions>
    {
    }
}
