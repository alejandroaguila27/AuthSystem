using AuthSystem.DataAccess.Configuration;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure
{
    public class AuthSystemDbContext : DbContext
    {
        public AuthSystemDbContext(DbContextOptions<AuthSystemDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de tablas y relaciones
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // Aquí puedes agregar datos iniciales si es necesario
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Email = "admin@example.com", PasswordHash = "hashed_password" }
            );
        }

    }
}


