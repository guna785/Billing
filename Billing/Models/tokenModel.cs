using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.Models
{
    public class tokenModel
    {
        public string unique_name { get; set; }
        public string role { get; set; }
        public long nbf { get; set; }
        public long exp { get; set; }
        public long iat { get; set; }
    }

}
