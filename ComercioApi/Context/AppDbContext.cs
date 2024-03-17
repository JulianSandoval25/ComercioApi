﻿using ComercioApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ComercioApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ComercioApi.Models.User> User { get; set; } = default!;
    }
}
