#nullable disable


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace dotnet_resources_api.Models
{
    public partial class resources_context : DbContext
    {
        public resources_context()
        {
        }

        public resources_context(DbContextOptions<resources_context> options)
            : base(options)
        {
        }
        //public virtual DbSet<classes> Classes { get; set; }
        public virtual DbSet<files> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

    }

}