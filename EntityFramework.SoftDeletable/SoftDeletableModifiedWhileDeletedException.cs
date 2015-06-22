using System;

namespace EntityFramework.SoftDeletable {
	public class SoftDeletableModifiedWhileDeletedException : InvalidOperationException {
		internal SoftDeletableModifiedWhileDeletedException() : base("Soft-deletable entities cannot be modified while they are marked deleted.") { }
	}
}