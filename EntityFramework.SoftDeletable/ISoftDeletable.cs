using System;
using EntityFramework.Triggers;

namespace EntityFramework.SoftDeletable {
    public interface ISoftDeletable : ITriggerable {
        DateTime? Deleted { get; }

        Action<DateTime?> DeletedPropertySetter { get; set; }
    }
}