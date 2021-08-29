using MvcStoreData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcStoreData
{
    public class DeliveryAddress : UserAddress
    {
        public string PhoneNumber { get; set; }

        public string ZipCode { get; set; }

    }
}
