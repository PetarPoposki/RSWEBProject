using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RSWEBProject.Areas.Identity.Data;

// Add profile data for application users by adding properties to the RSWEBProjectUser class
public class RSWEBProjectUser : IdentityUser
{
    public static implicit operator RSWEBProjectUser(List<RSWEBProjectUser> v)
    {
        throw new NotImplementedException();
    }
}

