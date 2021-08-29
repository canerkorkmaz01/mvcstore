using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcStoreData.Infrastructure
{
    public abstract class SortableBaseEntity : BaseEntity
    {
        [Display(Name = "Sıralama")]
        public int SortOrder { get; set; }
    }
}
