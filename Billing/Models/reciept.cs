using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.Models
{
    public class reciept
    {
        public payments py { get; set; }
        public client clt { get; set; }
        public sales sls { get; set; }
        public companyprofile prof { get; set; }
    }
}
