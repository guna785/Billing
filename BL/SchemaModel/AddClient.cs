using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class AddClient
    {
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-6")]
        public string name { get; set; }
        [GSchema("phone", "Enter Phone", "string", false, getHtmlClass = "col-md-6")]
        public string phone { get; set; }

        [GSchema("email", "Enter Email", "email", false, getHtmlClass = "col-md-6")]
        public string email { get; set; }
        [GSchema("gender", "Enter Gender", "string", true,getEnumVal ="Gender", getHtmlClass = "col-md-6")]
        public string gender { get; set; }
        [GSchema("address", "Enter Address", "string", true, getHtmlClass = "col-md-6")]
        public string address { get; set; }
        [GSchema("pan", "Enter PAN", "string", true, getRegularExpression = "[A-Z]{5}[0-9]{4}[A-Z]{1}", getmessage = true, getHtmlClass = "col-md-6")]
        public string pan { get; set; }
        [GSchema("gst", "Enter GST", "string", false, getRegularExpression = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$",
             getmessage = true, getHtmlClass = "col-md-6")]
        public string gst { get; set; }
        [GSchema("state", "Enter State", "string",true,getEnumVal = "State", getHtmlClass = "col-md-6")]
        public string state { get; set; }
    }
}
