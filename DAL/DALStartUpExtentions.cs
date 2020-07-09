using DAL.DALRepo;
using DAL.DALService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public static  class DALStartUpExtentions
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminRepo, AdminRepo>();
            services.AddScoped<IBranchRepo, BranchRepo>();
            services.AddScoped<ICatagoryRepo, CatagoryRepo>();
            services.AddScoped<IClientRepo, ClientRepo>();
            services.AddScoped<ICompanyProfileRepo, CompanyProfileRepo>();
            services.AddScoped<IInvoiceRepo, InvoiceRepo>();
            services.AddScoped<ILogRepo, LogRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IProductCompanyRepo, ProductCompanyRepo>();
            services.AddScoped<IProductTypeRepo, ProductTypeRepo>();
            services.AddScoped<IpurchaseRepo, PurchaseRepo>();
            //services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<ISalesRepo, SalesRepo>();
            services.AddScoped<IStocksRepo, StocksRepo>();
            services.AddScoped<ISuplierRepo, SuplierRepo>();

            services.AddScoped<IAthenticate, Athenticate>();
            services.AddScoped<IuserRepo, UserRepo>();

            return services;
        }
    }
}
