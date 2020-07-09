using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class suplier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }
        public string cpname { get; set; }

        public string cphone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        [Required(ErrorMessage = "PAN can not be Null")]
        public string pan { get; set; }
        public string gst { get; set; }

        public DateTime cdate { get; set; }

        public string status { get; set; }
    }
}
