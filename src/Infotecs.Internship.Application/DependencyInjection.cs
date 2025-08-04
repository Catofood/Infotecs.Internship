using System.Reflection;
using FluentValidation;
using Infotecs.Internship.Application.Behaviors;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Application.Services;
using Infotecs.Internship.Application.Services.Csv;
using Infotecs.Internship.Application.Services.OperationsResultFactory;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infotecs.Internship.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();

        services.Configure<ValidationOptions>(configuration.GetSection(ValidationOptions.ConfigurationSectionName));
        services.AddTransient<IOperationsResultFactory, OperationsResultFactory>();
        services.AddScoped<IOperationsFileCsvService, OperationsFileCsvService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(thisAssembly));
        services.AddValidatorsFromAssembly(thisAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}