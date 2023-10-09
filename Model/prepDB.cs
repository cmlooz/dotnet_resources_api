using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace dotnet_resources_api.Models
{
    public static class prepDB
    {
        public static void prepareDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<resources_context>());
            }

        }

        public static void seedData(resources_context context)
        {
            System.Console.WriteLine("Appling Migrations....");
            context.Database.Migrate();
            System.Console.WriteLine("Migrations Applied....");

        }
    }

}
