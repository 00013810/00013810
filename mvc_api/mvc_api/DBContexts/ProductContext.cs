using Microsoft.EntityFrameworkCore;
using mvc_api.Model;
using System.Collections.Generic;

namespace mvc_api.DBContexts
{
        public class ProductContext : DbContext
        {
            // Constructors DO NOT FORGET That Product Context class extends DB Context
            public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
        }
}
