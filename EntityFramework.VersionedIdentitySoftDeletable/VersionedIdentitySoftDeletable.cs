using System;
using EntityFramework.VersionedSoftDeletable;
using Microsoft.AspNet.Identity;

namespace EntityFramework.VersionedIdentitySoftDeletable
{
    public class VersionedIdentitySoftDeletable<TUserId, TVersionedUserDeleted> : VersionedUserSoftDeletable<TUserId, TVersionedUserDeleted> where TUserId : IConvertible {
	    public sealed override TUserId GetCurrentUserId() {
		    return System.Web.HttpContext.Current.User.Identity.GetUserId<TUserId>();
	    }
    }
}
