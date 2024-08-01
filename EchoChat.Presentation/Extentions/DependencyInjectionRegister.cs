using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.UserAggregates;
using EchoChat.Infrastructure.DataAccess;
using EchoChat.Infrastructure.DataAccess.Firebase.FirebaseStorage;
using EchoChat.Infrastructure.DataAccess.Firebase.Firestore.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EchoChat.Extentions;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10 mb
        });
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        });

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IFirebseStorageService, FirebaseStorageService>();
        services.AddScoped<IFirestoreClientFactory, FirestoreClientFactory>();
        services.AddScoped<ICollectionReferenceFactory, CollectionReferenceFactory>();

        return services;
    }
}