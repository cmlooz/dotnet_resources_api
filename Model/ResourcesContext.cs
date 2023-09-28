#nullable disable


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ResourcesAPI.Models
{
    public partial class ResourcesContext : DbContext
    {
        public ResourcesContext()
        {
        }

        public ResourcesContext(DbContextOptions<ResourcesContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Files> Files { get; set; }

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