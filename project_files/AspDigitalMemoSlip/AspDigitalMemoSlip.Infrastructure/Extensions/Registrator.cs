using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using AspDigitalMemoSlip.Infrastructure.Repositories;
using AspDigitalMemoSlip.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AspDigitalMemoSlip.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.RegisterRepositories();

            return services;
        }

        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MemoSlipContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DigitalMemoSlip")));

            return services;
        }


        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IConsigneeRepository, ConsigneeRepository>();
            services.AddScoped<IConsignerRepository, ConsignerRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IGenericNotificationRepository, GenericNotificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMemoRepository, MemoRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISalesConfirmationRepository, SalesConfirmationRepository>();
            services.AddScoped<IProductSaleRepository, ProductSaleRepository>();

            return services;
        }


    }
}
