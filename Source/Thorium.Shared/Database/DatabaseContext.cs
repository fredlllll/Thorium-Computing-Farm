using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Text.Json;
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

            modelBuilder.Entity<Node>().HasOne<Task>().WithOne().HasForeignKey<Node>(x => x.CurrentTaskId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Task>().HasOne<Node>().WithMany().HasForeignKey(x => x.LinedUpOnNodeId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Task>().HasOne<Job>().WithMany().HasForeignKey(x=>x.JobId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Operation>().HasOne<Job>().WithMany().HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = false };
#pragma warning disable CS8603 //as the field isnt nullable, we should never be able to receive null on deserialization
            modelBuilder.Entity<Operation>().Property(x => x.Data).HasColumnType("jsonb").HasConversion(y => JsonSerializer.Serialize(y, options), z => JsonSerializer.Deserialize<Dictionary<string, string>>(z, options));
#pragma warning restore CS8603


        }

        public static string GetNewId<T>()
        {
            var t = typeof(T);
            var typeId = t.Name.ToLower();
            var id = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
            return $"{typeId}_{id}";
        }

        public static DbContextOptionsBuilder<DatabaseContext> GetOptionsBuilder(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var t = optionsBuilder.UseNpgsql(connectionString, o =>
            {
                o.MapEnum<TaskStatus>("task_status");
            });
            return optionsBuilder;
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(DatabaseConnection.LoadFromSettings().GetConnectionString());
        }*/

        public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
        {
            public DatabaseContext CreateDbContext(string[] args)
            {
                var optionsBuilder = GetOptionsBuilder(DatabaseConnection.LoadFromSettings().GetConnectionString());

                return new DatabaseContext(optionsBuilder.Options);
            }
        }
    }
}
