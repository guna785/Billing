using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class CompanyProfileEdit
    {
        [GSchema("Id", "Enter ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
        [GSchema("address", "Enter Address", "string", true,getHtmlClass = "col-md-6")]
        public string address { get; set; }
        [GSchema("email", "Enter Email", "email", true, getHtmlClass = "col-md-6")]
        public string email { get; set; }
        [GSchema("phone", "Enter Phone", "number", true, getHtmlClass = "col-md-6",getMaximun ="9999999999",getminimum ="1000000000")]
        public string phone { get; set; }
        [GSchema("state", "Select Sate", "string", true, getEnumVal = "State", getHtmlClass = "col-md-6",getfieldHtmlClass = "select2 selectfield")]
        public string state { get; set; }
        [GSchema("web", "Enter WebSite", "string", getHtmlClass = "col-md-6")]
        public string web { get; set; }

        [GSchema("pan", "Enter PAN", "string", true, getRegularExpression = "[A-Z]{5}[0-9]{4}[A-Z]{1}", getmessage = true, getHtmlClass = "col-md-6")]
        public string pan { get; set; }
        [GSchema("tin", "Enter TIN", "string", getHtmlClass = "col-md-6")]
        public string tin { get; set; }
        [GSchema("gst", "Enter GST", "string", true, getRegularExpression = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$",
            getmessage =true, getHtmlClass = "col-md-6")]
        public string gst { get; set; }
        [GSchema("photo", "Browse Logo", "file", getHtmlClass = "col-md-6")]
        public string photo { get; set; }
        [GSchema("hsn", "Enter hsn", "string", true, getHtmlClass = "col-md-6")]
        public string hsn { get; set; }
        [GSchema("bankname", "Enter Bank Name", "string", getHtmlClass = "col-md-6")]
        public string bankname { get; set; }
        [GSchema("bankaccno", "Enter Bank Account NO", "string", true, getHtmlClass = "col-md-6")]
        public string bankaccno { get; set; }
       
        [GSchema("bankbranch", "Enter Bank Branch", "string", true, getHtmlClass = "col-md-6")]
        public string bankbranch { get; set; }
        [GSchema("ifsc", "Enter ifsc", "string", true, getHtmlClass = "col-md-6")]
        public string ifsc { get; set; }
    }
}
