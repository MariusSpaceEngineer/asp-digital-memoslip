using AspDigitalMemoSlip.Application.Behavior;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Application.Interfaces.Services.Authentication;
using AspDigitalMemoSlip.Application.Services.Authentication;
using AspDigitalMemoSlip.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AspDigitalMemoSlip.Application.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<TokenHelper>();
            services.AddScoped<IUserTwoFactorTokenProvider<User>, AuthenticatorTokenProvider<User>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IEmailService, MailjetEmailService>();

            return services;

        }

    }
}
