using System;

namespace EntityFramework.SoftDeletable {
	public class SoftDeletableMarkedDeletedIncorrectly : InvalidOperationException {
		internal SoftDeletableMarkedDeletedIncorrectly() : base("Soft-deletable entities cannot be marked as deleted before the previous mark") { }
	}
}