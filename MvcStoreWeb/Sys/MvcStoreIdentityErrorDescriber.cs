using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Sys
{
    public class MvcStoreIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = "TooShort", Description = $"Parolanız en az {length} karakter olmalıdır!" };
        }

    }
}
