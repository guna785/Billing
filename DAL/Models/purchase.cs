using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class purchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string invid { get; set; }
        public List<purchaseitem> content { get; set; }
        public string tottax { get; set; }
        public string suplier { get; set; }
      
        public string tamt { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public bool isTaxed { get; set; }

        public DateTime cdate { get; set; }
        public DateTime invdate { get; set; }
    }
}
