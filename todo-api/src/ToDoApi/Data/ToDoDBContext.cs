using Contoso.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contoso.ToDo.Api.Data
{
    public class ToDoDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<ToDoItem> ToDos {get;set;}

        public ToDoDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(_configuration["ToDoDBConnection"],_configuration["ToDoDBName"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>()
                .ToContainer(_configuration["ToDoCollection"])
                .HasPartitionKey(item => item.User);
        }
    }
}
