using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcStoreData.Infrastructure
{
    public interface IUserAddress
    {
        int Id { get; set; }

        int UserId { get; set; }

        string Name { get; set; }

        string Address { get; set; }

        int CityId { get; set; }

        User User { get; set; }
    }
}
