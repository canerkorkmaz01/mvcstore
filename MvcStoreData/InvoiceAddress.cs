using MvcStoreData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcStoreData
{
    public class InvoiceAddress : UserAddress
    {
        public string TaxOffice { get; set; }

        public string TaxNumber { get; set; }


    }
}
