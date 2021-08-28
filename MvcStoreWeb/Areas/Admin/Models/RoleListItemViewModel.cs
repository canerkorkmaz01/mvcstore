using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Areas.Admin.Models
{
    public class RoleListItemViewModel
    {
        public Role Role { get; set; }

        public int UserId { get; set; }

        public string CurrentRoleName { get; set; }
    }
}
