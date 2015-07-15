using EntityFramework.VersionedProperties;

namespace EntityFramework.VersionedSoftDeletable {
	public interface IVersionedSoftDeletable : IVersionedProperties {
		VersionedBoolean Deleted { get; }
	}
}
