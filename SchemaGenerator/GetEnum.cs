using DAL.DbService;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    class GetEnum
    {
    }
    public static class getEnumList
    {
        public async static Task<string> getEnumRecords(string val)
        {
            BillingService _service = new BillingService();
            if (val.Equals("Gender"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>() { "Male", "Female", "Others" });
            }
            else if (val.Equals("Role"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>() { "User", "Admin" });
            }
            else if (val.Equals("State"))
            {
                var res = _service.statecodes.Find(x => true).ToList().Select(x => x.name).ToList();
                return Newtonsoft.Json.JsonConvert.SerializeObject( new List<string>()
                {
                    "Andaman and Nicobar Islands",
                    "Andhra Pradesh",
                    "Arunachal Pradesh",
                    "Assam",
                    "Bihar",
                    "Chandigarh",
                    "Chhattisgarh",
                    "Dadra and Nagar Haveli",
                    "Daman and Diu",
                    "Delhi",
                    "Goa",
                    "Gujarat",
                    "Haryana",
                    "Himachal Pradesh",
                    "Jammu and Kashmir",
                    "Jharkhand",
                    "Karnataka",
                    "Kerala",
                    "Ladakh",
                    "Lakshadweep",
                    "Madhya Pradesh",
                    "Maharashtra",
                    "Manipur",
                    "Meghalaya",
                    "Mizoram",
                    "Nagaland",
                    "Odisha",
                    "Puducherry",
                    "Punjab",
                    "Rajasthan",
                    "Sikkim",
                    "Tamil Nadu",
                    "Telangana",
                    "Tripura",
                    "Uttar Pradesh",
                    "Uttarakhand",
                    "West Bengal"
                }.Distinct());
            }
            else if (val.Equals("company"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(_service.productcompanys.Find(x => true).ToList().Select(x => x.name).ToList());
            }
            else if (val.Equals("category"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(_service.categorys.Find(x => true).ToList().Select(x => x.name).ToList());
            }
            else if (val.Equals("procudttype"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(_service.producttypes.Find(x => true).ToList().Select(x => x.name).ToList());
            }
            return "";
        }

        public async static Task<string> getVlidationMessage(string val)
        {
            var msg = new
            {
                required = val + " is Required Property",
                pattern = "Correct format of " + val

            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(msg);
        }
    }
}
