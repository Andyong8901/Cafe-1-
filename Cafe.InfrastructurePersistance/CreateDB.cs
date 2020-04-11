using Cafe.DomainModelEntity;
using System.Data.Entity;

namespace Cafe.InfrastructurePersistance
{
    public class CreateDB:DbContext
    {
        public CreateDB():base("Cafe2020")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<OrderCart> OrderCarts { get; set; }
        public DbSet<Table> Tables { get; set; }
    }
}