using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Registration> Registers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonTime> LessonsTime { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=AppDb;Integrated Security=True",
                    options => options.MigrationsAssembly("Api"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
               .HasKey(s => s.Id);

            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Lesson>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<LessonTime>()
                .HasKey(lt => lt.Id);

            modelBuilder.Entity<Registration>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Class>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Faculty>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.Id);


            modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Departments)
            .WithOne(d => d.Faculty)
            .HasForeignKey(d => d.FacultyId);

            modelBuilder.Entity<Department>()
            .HasMany(d => d.Teachers)
            .WithOne(t => t.Department)
            .HasForeignKey(t => t.DepartmentId);

            modelBuilder.Entity<Department>()
            .HasMany(d => d.Students)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmenId);

            modelBuilder.Entity<Department>()
            .HasMany(d => d.Lessons)
            .WithOne(l => l.Department)
            .HasForeignKey(l => l.DepartmentId);

            modelBuilder.Entity<Department>()
            .HasMany(d => d.Classes)
            .WithOne(c => c.Department)
            .HasForeignKey(c => c.DepartmenId);

            modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Teachers)
            .WithOne(t => t.Faculty)
            .HasForeignKey(t => t.FacultyId);

            modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Students)
            .WithOne(s => s.Faculty)
            .HasForeignKey(s => s.FacultyId);

            modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Lessons)
            .WithOne(l => l.Faculty)
            .HasForeignKey(l => l.FacultyId);

            modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Classes)
            .WithOne(c => c.Faculty)
            .HasForeignKey(c => c.FacultyId);

            modelBuilder.Entity<Lesson>()
            .HasMany(l => l.Registrations)
            .WithOne(r => r.Lesson)
            .HasForeignKey(r => r.LessonId);

            modelBuilder.Entity<Student>()
            .HasMany(s => s.Registrations)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<Teacher>()
            .HasMany(t => t.Registrations)
            .WithOne(r => r.Teacher)
            .HasForeignKey(r => r.TeacherId);

            modelBuilder.Entity<Class>()
            .HasMany(c => c.Lessons)
            .WithMany(l => l.Classes)
            .UsingEntity(j => j.ToTable("ClassLesson"));

            modelBuilder.Entity<Lesson>()
            .HasMany(l => l.Students)
            .WithMany(s => s.Lessons)
            .UsingEntity(j => j.ToTable("LessonStudent"));

            modelBuilder.Entity<Lesson>()
            .HasMany(l => l.Teachers)
            .WithOne(t => t.Lesson)
            .HasForeignKey(t => t.LessonId);

            modelBuilder.Entity<LessonTime>()
            .HasMany(lt => lt.Lessons)
            .WithMany(l => l.LessonsTime)
            .UsingEntity(j => j.ToTable("LessonLessonTime"));

            modelBuilder.Entity<Class>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Classes)
            .UsingEntity(j => j.ToTable("ClassStudent"));

            modelBuilder.Entity<Class>()
            .HasMany(c => c.LessonsTime)
            .WithMany(lt => lt.Classes)
            .UsingEntity(j => j.ToTable("ClassLessonTime"));


            //modelBuilder.Entity<LessonTime>()
            //.HasMany(lt => lt.Lessons);  zaten ICollection olarak belirttin

            //modelBuilder.Entity<Lesson>()
            //.HasMany(l => l.LessonsTime);  zaten ICollection olarak belirttin

            //modelBuilder.Entity<Student>()
            //.HasMany(s => s.Classes);  zaten ICollection olarak belirttin

            //modelBuilder.Entity<Class>()
            //.HasMany(c => c.Students);  zaten ICollection olarak belirttin

            //modelBuilder.Entity<Class>()
            //.HasMany(c => c.LessonsTime);  zaten ICollection olarak belirttin

            //modelBuilder.Entity<LessonTime>()
            //.HasMany(lt => lt.Classes);  zaten ICollection olarak belirttin

        }
    }
}
