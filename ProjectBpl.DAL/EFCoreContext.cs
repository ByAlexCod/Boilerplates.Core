using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProjectBpl.DAL.Entities;

namespace ProjectBpl.DAL
{
    class EFCoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable("BPL_CONNECTION_STRING_ENV");
            if (connectionString == null)
            {
                throw new ArgumentException("Could not find any env variable named BPL_CONNECTION_STRING_ENV . Please set it before launching the migration.");
            }
            optionsBuilder.UseSqlServer(connectionString);
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });
        }
    }
}
