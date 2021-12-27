using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketplaceAPI.Models
{
    public class MarketplaceContext:DbContext
    {
        public MarketplaceContext(DbContextOptions<MarketplaceContext>options):base(options)
        {

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Payment> Payment { get; set; }
    }
}
