using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Application.Security.Permissions
{
    public partial class PermissionAttribute
    {
        public class PermissionRequirement : IAuthorizationRequirement
        {
            public string Permission { get; }

            public PermissionRequirement(string permission)
            {
                Permission = permission;
            }
        }
    }
}
