using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class companyprofile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        [Required(ErrorMessage = "PAN can not be Null")]
        public string pan { get; set; }
        public string tin { get; set; }
        public string gst { get; set; }
        
        public byte[] photo { get; set; }
        public string hsn { get; set; }
        public string bankname { get; set; }
        public string bankaccno { get; set; }
        public string state { get; set; }
        public string bankbranch { get; set; }
        public string ifsc { get; set; }
    }
}
