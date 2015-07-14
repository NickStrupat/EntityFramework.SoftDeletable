using System;
using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable
{
    public abstract class VersionedSoftDeletable : IVersionedSoftDeletable
    {
        public VersionedBoolean Deleted { get; internal set; }

        protected VersionedSoftDeletable()
        {
            this.InitializeVersionedSoftDeletable();
        }
    }
}