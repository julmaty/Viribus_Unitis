using Microsoft.EntityFrameworkCore;
namespace Viribus_Unitis
{
    using Microsoft.EntityFrameworkCore;
    using Viribus_Unitis.Models;

    public class ApplicationContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Документы" },
            new Tag { Id = 2, Name = "Учеба" },
            new Tag { Id = 3, Name = "Реквизит" },
            new Tag { Id = 4, Name = "Досуг" },
            new Tag { Id = 5, Name = "Мероприятия" },
            new Tag { Id = 6, Name = "Наука" },
            new Tag { Id = 7, Name = "Развлечения" },
            new Tag { Id = 8, Name = "Общежитие" },
            new Tag { Id = 9, Name = "Разное" },
            new Tag { Id = 10, Name = "Срочно" });


        }
    }
}
