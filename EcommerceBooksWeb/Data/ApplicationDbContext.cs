using EcommerceBooksWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EcommerceBooksWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
