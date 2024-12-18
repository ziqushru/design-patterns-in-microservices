using System;
using System.Threading;
using Contracts.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Contracts.WebApi.Managers;

public static class MigrationsManager
{
    private static int _numberOfRetries;

    public static IApplicationBuilder UseMigrationsManager(this IApplicationBuilder host)
    {
        using var scope = host.ApplicationServices.CreateScope();
        using var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        try
        {
            applicationContext.Database.Migrate();
        }
        catch (MySqlException exception)
        {
            if (_numberOfRetries >= 3)
            {
                throw;
            }

            Thread.Sleep(10000);

            _numberOfRetries++;

            Console.WriteLine($"The server was not found or was not accessible. Retrying... #{_numberOfRetries}");
            Console.WriteLine(exception);

            host.UseMigrationsManager();

            throw;
        }

        return host;
    }
}
