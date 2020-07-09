using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class EditCategory
    {
        [GSchema("Id", "Enter ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
    }
}
