using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class PayInvoice
    {
        [GSchema("Id", "Enter ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("amount", "Enter Amount", "number", true,null,null,"0", getHtmlClass = "col-md-12")]
        public string amount { get; set; }
    }
}
