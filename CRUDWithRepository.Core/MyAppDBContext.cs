using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class MyAppDBContext : DbContext
    {
        public MyAppDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<LoomMachine> LoomMachine { get; set; }
        public DbSet<WarpingMachine> WarpingMachine { get; set; }
        public DbSet<User> UserMaster { get; set; }
        public DbSet<WarpingPlan> WarpingPlanMaster { get; set; }
        public DbSet<WarpingPlanDetails> WarpingPlanDetails { get; set; }
        public DbSet<ERPProductDTO> ERPProductResults { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ERPProductDTO as keyless
            modelBuilder.Entity<ERPProductDTO>().HasNoKey();
        }
    }
}
