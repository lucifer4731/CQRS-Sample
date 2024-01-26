using CQRS.Domain.Base;
using CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Context
{
    public class CQRSContext : DbContext
    {
        public CQRSContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<User>().HasData(
        //        new User
        //        {
        //            Id = Guid.NewGuid(),
        //            FullName = "Admin",
        //            CreateDate = DateTime.Now,
        //            Description = "Admin user Created on first run",
        //            Password = "Admin123",
        //            UserName = "Admin",
        //            Deleted = false,
        //        });
        //}

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
