using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class AddEmployee
    {
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-6")]
        public string name { get; set; }
        [GSchema("uname", "Enter UserName", "string", true, getHtmlClass = "col-md-6")]
        public string uname { get; set; }
        [GSchema("password", "Enter password", "password", true, getHtmlClass = "col-md-6")]
        public string password { get; set; }
        [GSchema("email", "Enter Email", "email", true, getHtmlClass = "col-md-6")]
        public string email { get; set; }
        [GSchema("phone", "Enter Phone", "string", true, getHtmlClass = "col-md-6")]
        public string phone { get; set; }
        [GSchema("address", "Enter Address", "string", true, getHtmlClass = "col-md-6")]
        public string address { get; set; }
        [GSchema("role", "Enter Role", "string", true, getEnumVal = "Role", getHtmlClass = "col-md-6")]
        public string role { get; set; }
        [GSchema("joindate", "Browse joindate", "date", getHtmlClass = "col-md-6")]
        public string joindate { get; set; }
        [GSchema("dob", "Enter Minimum dob", "date", true, getHtmlClass = "col-md-6")]
        public string dob { get; set; }
        [GSchema("remarks", "Enter remarks", "string", true, getHtmlClass = "col-md-6")]
        public string remarks { get; set; }
        
    }
}
