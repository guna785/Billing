using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class EditSuplier
    {
        [GSchema("Id", "Enter ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
        [GSchema("cpname", "Enter Contact Person Name", "string", true, getHtmlClass = "col-md-6")]
        public string cpname { get; set; }

        [GSchema("email", "Enter Email", "email", true, getHtmlClass = "col-md-6")]
        public string email { get; set; }
        [GSchema("cphone", "Enter Phone", "string", true, getHtmlClass = "col-md-6")]
        public string cphone { get; set; }
        [GSchema("address", "Enter Address", "string", true, getHtmlClass = "col-md-6")]
        public string address { get; set; }
        [GSchema("pan", "Enter PAN", "string", true, getRegularExpression = "[A-Z]{5}[0-9]{4}[A-Z]{1}", getmessage = true, getHtmlClass = "col-md-6")]
        public string pan { get; set; }
        [GSchema("gst", "Enter GST", "string", true, getRegularExpression = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$",
            getmessage = true, getHtmlClass = "col-md-6")]
        public string gst { get; set; }
    }
}
