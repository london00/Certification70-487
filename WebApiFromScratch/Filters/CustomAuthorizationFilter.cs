using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiFromScratch.Filters
{
    public class AuthorizationBlackListFilter : AuthorizeAttribute
    {
        public AuthorizationBlackListFilter()
        {
            Roles = "Under18Users";
            Users = "Little Timmy";
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IPrincipal user = Thread.CurrentPrincipal;

            if (user == null) {
                return true;
            }

            var splitUsersBlackList = SplitString(Users);
            if (splitUsersBlackList.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase)) {
                return false;
            }

            var splitRolesBlackList = SplitString(Roles);
            return splitRolesBlackList.Any(user.IsInRole);
        }

        private static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}