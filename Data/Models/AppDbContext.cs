using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonTime> LessonsTime { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }
        public DbSet<DaysOfWeek> DaysOfWeeks { get; set; }
        public DbSet<LessonSchedule> LessonSchedules { get; set; }
        public DbSet<RegisterRequest> RegisterRequests { get; set; }
        public DbSet<Faculty> Faculties { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=AppDb;Integrated Security=True",
                    options => options.MigrationsAssembly("Data"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
