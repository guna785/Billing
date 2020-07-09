using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }

        public string cmname { get; set; }
        public string company { get; set; }
        public string category { get; set; }
        public string procudttype { get; set; }
        public string pid { get; set; }
        public string mrp { get; set; }
        public string discount { get; set; }
        public string qty { get; set; }
        public string tax { get; set; }
        public DateTime cdate { get; set; }
        public DateTime lmdate { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public byte[] photo { get; set; }
        public string minalert { get; set; }
        public string warranty { get; set; }
        public string actprice { get; set; }
        public string mrcode { get; set; }
        public string nonTaxQty { get; set; }
    }
}
