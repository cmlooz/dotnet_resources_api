using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ResourcesAPI.Models
{
    public static class prepDB
    {
        public static void prepareDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<ResourcesContext>());
            }

        }

        public static void seedData(ResourcesContext context)
        {
            System.Console.WriteLine("Appling Migrations....");
            context.Database.Migrate();
            System.Console.WriteLine("Migrations Applied....");

        }
    }

}
