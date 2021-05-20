using System;
using System.Collections.Generic;
using System.Text;
using Boilerplates.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Boilerplates.DAL
{
    class EFCoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Boilerplate> Boilerplates { get; set; }
        public DbSet<Placeholder> Placeholders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\;Database=EFCoreWebDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
