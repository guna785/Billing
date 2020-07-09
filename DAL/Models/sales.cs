using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class sales
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string sid { get; set; }
        public List<salesitem> content { get; set; }
        public string tamt { get; set; }
        public string taxt { get; set; }
        public string status { get; set; }
        public string clid { get; set; }
        public string clname { get; set; }
        public string userid { get; set; }
        public DateTime cdate { get; set; }
        public string gst { get; set; }
        public bool isTaxed { get; set; }
    }
}
