using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface IAuthorizer
    {
        string GetToken(IdentityUser user);

        IdentityUser GetUser(string token);
    }
}
