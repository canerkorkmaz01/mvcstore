using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcStoreData.Infrastructure
{
    public class SortableBaseEntity : BaseEntity
    {
        public int SortOrder { get; set; }
    }
}
