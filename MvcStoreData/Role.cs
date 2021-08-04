using Microsoft.AspNetCore.Identity;

namespace MvcStoreData
{
    public class Role : IdentityRole<int>
    {
        public string DisplayName { get; set; }
    }
}


