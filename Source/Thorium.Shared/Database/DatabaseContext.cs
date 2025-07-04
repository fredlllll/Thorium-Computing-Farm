using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Thorium.Shared.Database.Models;
using UUIDNext;

namespace Thorium.Shared.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Node> Nodes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>().ToTable(nameof(Jobs));
            modelBuilder.Entity<Operation>().ToTable(nameof(Operations));
            modelBuilder.Entity<Task>().ToTable(nameof(Tasks));
            modelBuilder.Entity<Node>().ToTable(nameof(Nodes));

            modelBuilder.Entity<Node>().HasOne<Task>(x => x.Task).WithMany().HasForeignKey("TaskId");
            modelBuilder.Entity<Task>().HasOne<Node>(x => x.CurrentMachine).WithMany().HasForeignKey("CurrentMachineId");

            modelBuilder.Entity<Operation>().Property(x => x.Data).HasColumnType("jsonb");
        }

        public static string GetNewId<T>()
        {
            var t = typeof(T);
            var typeId = t.Name.ToLower();
            var id = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
            return $"{typeId}_{id}";
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(DatabaseConnection.LoadFromSettings().GetConnectionString());
        }*/

        public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
        {
            public DatabaseContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseNpgsql(DatabaseConnection.LoadFromSettings().GetConnectionString());

                return new DatabaseContext(optionsBuilder.Options);
            }
        }
    }
}
