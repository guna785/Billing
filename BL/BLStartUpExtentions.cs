using BL.BLRepo;
using BL.BLService;
using BL.SchemaEditBuilder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public static class BLStartUpExtentions
    {
        public static IServiceCollection AddBLServices(this IServiceCollection services)
        {
            services.AddScoped<IStocksRepository, StockRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IClientRepostory, ClientRepository>();
            services.AddScoped<ICompanyProfileRepository, ComponyProfileRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IPymentRepositroy, PaymentRepository>();
            services.AddScoped<IProductCompanyRepository, ProductCompanyRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<ISuplierRepository, SuplierRepository>();
            services.AddScoped<EditBuilder>();
            //services.AddScoped<IRoleRepository, RoleRepository>();
            return services;
        }
    }
}
