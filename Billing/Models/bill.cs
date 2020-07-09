using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace Billing.Models
{
    public class bill
    {
        public invoice inv { get; set; }
        public client clt { get; set; }
        public sales sls { get; set; }
        public companyprofile prof { get; set; }
    }
}
