using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StudieTips.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _RM;
        RoleService(RoleManager<IdentityRole> RM)
        {
            _RM = RM;
        }
    }
}
