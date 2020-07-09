using SchemaGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class EditStock
    {
        [GSchema("Id", "Enter ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Enter Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
        [GSchema("cmname", "Enter Common Name", "string", true, getHtmlClass = "col-md-6")]
        public string cmname { get; set; }
        [GSchema("category", "Enter category", "string", true, getEnumVal = "category", getHtmlClass = "col-md-6")]
        public string category { get; set; }
        [GSchema("procudttype", "Enter procudttype", "string", true, getEnumVal = "procudttype", getHtmlClass = "col-md-6")]
        public string procudttype { get; set; }
        [GSchema("company", "Enter Company Name", "string", true, getEnumVal = "company", getHtmlClass = "col-md-6")]
        public string company { get; set; }
        [GSchema("mrp", "Enter MRP", "string", true, getHtmlClass = "col-md-6")]
        public string mrp { get; set; }
        [GSchema("discount", "Enter Discount", "string", true, getHtmlClass = "col-md-6")]
        public string discount { get; set; }
        [GSchema("qty", "Enter Quanty", "string", true, getHtmlClass = "col-md-6")]
        public string qty { get; set; }
        [GSchema("tax", "Enter Tax", "string", true, getHtmlClass = "col-md-6")]
        public string tax { get; set; }
        [GSchema("photo", "Browse For Photo", "file", getHtmlClass = "col-md-6")]
        public string photo { get; set; }
        [GSchema("minalert", "Enter Minimum Stock Alert", "string", true, getHtmlClass = "col-md-6")]
        public string minalert { get; set; }
        [GSchema("warranty", "Enter Warranty", "string", true, getHtmlClass = "col-md-6")]
        public string warranty { get; set; }
        [GSchema("actprice", "Enter Actual Price", "string", true, getHtmlClass = "col-md-6")]
        public string actprice { get; set; }
    }
}
