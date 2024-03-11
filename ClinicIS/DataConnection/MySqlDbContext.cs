using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ClinicIS.Models;

namespace produkto.DataConnection
{
    public class MySqlDbContext:DbContext
    {
 

        public DbSet<User> Users { get; set; }

        public DbSet<Inventory_Items> Inventory_Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
    }
}
