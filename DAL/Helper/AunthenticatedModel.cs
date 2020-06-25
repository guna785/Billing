using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Helper
{
    public class AunthenticatedModel
    {
        public string uname { get; set; }
        public string role { get; set; }
        public JwtToken Token { get; set; }
    }
}
