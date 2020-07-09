using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.Models
{
    public class PurchaseMake
    {
        public string invno { get; set; }
        public DateTime invdate { get; set; }
        public string suplier { get; set; }
        public string tax { get; set; }
        public string amt { get; set; }
        public string comment { get; set; }
    }
}
