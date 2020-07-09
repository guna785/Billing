using DAL.Helper;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DbService
{
    public class BillingService
    {
        public BillingService()
        {
            var appConfig = new ApplicationCofigaration();
            var client = new MongoClient(appConfig.connectionString);
            var database = client.GetDatabase(appConfig.DB);
            users = database.GetCollection<user>("user");
            admins = database.GetCollection<admin>("admin");
            clients = database.GetCollection<client>("client");
            supliers = database.GetCollection<suplier>("suplier");
            companyprofiles = database.GetCollection<companyprofile>("companyprofile");
            stocks = database.GetCollection<stock>("stock");
            branchs = database.GetCollection<branch>("MeterialType");
            categorys = database.GetCollection<category>("category");
            invoicess = database.GetCollection<invoice>("invoice");
            purchases = database.GetCollection<purchase>("purchase");
            saless = database.GetCollection<sales>("sales");
            paymentss = database.GetCollection<payments>("payments");
            productcompanys = database.GetCollection<productcompany>("productcompany");
            producttypes = database.GetCollection<producttype>("producttype");
            roles = database.GetCollection<role>("role");
            logs = database.GetCollection<log>("log");
            statecodes = database.GetCollection<statecode>("statecode");

        }
        public IMongoCollection<statecode> statecodes { get; set; }
        public IMongoCollection<user> users { get; set; }
        public IMongoCollection<admin> admins { get; set; }
        public IMongoCollection<client> clients { get; set; }
        public IMongoCollection<suplier> supliers { get; set; }
        public IMongoCollection<companyprofile> companyprofiles { get; set; }
        public IMongoCollection<stock> stocks { get; set; }
        public IMongoCollection<branch> branchs { get; set; }
        public IMongoCollection<category> categorys { get; set; }
        public IMongoCollection<invoice> invoicess { get; set; }
        public IMongoCollection<payments> paymentss { get; set; }
        public IMongoCollection<sales> saless { get; set; }
        public IMongoCollection<purchase> purchases { get; set; }
        public IMongoCollection<productcompany> productcompanys { get; set; }
        public IMongoCollection<producttype> producttypes { get; set; }
        public IMongoCollection<role> roles { get; set; }
        public IMongoCollection<log> logs { get; set; }

    }
}
