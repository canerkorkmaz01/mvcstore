using Microsoft.AspNetCore.Identity;

namespace MvcStoreData
{

    public enum Genders
    {
        Male, Female, Unspecified
    }


    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public Genders Gender { get; set; }

    }
}