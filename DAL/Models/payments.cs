using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class payments
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string sid { get; set; }
        public string userid { get; set; }
        public string pid { get; set; }
        public string amt { get; set; }
        public string invid { get; set; }
        public string status { get; set; }
        public DateTime cdate { get; set; }
        public bool isTaxed { get; set; }
    }
}
