using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.Models
{
    public class PurchaseModel
    {
        public string Id { get; set; }
        public string name { get; set; }

        public string cmname { get; set; }
        public string company { get; set; }
        public string mrp { get; set; }
        public string discount { get; set; }
        public int qty { get; set; }
        public string tax { get; set; }
        public byte[] photo { get; set; }
        public string minalert { get; set; }
        public string warranty { get; set; }
        public string actprice { get; set; }
        public string amount { get; set; }
    }
}
