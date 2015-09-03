using System;

namespace EntityFramework.SoftDeletable {
	public abstract class SoftDeletable : ISoftDeletable {
		public DateTime? Deleted { get; internal set; }

		protected SoftDeletable() {
			this.InitializeSoftDeletable();
		}
	}
}