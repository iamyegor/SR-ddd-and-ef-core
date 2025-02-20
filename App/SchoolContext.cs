﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App;

public sealed class SchoolContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;
    private readonly EventDispatcher _eventDispatcher;

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext(
        string connectionString,
        bool useConsoleLogger,
        EventDispatcher eventDispatcher
    )
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
        _eventDispatcher = eventDispatcher;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter(
                    (category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information
                )
                .AddConsole();
        });

        optionsBuilder.UseSqlServer(_connectionString).UseLazyLoadingProxies();

        if (_useConsoleLogger)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("StudentID");
            x.ComplexProperty(
                p => p.Email,
                emailBuilder =>
                {
                    emailBuilder.Property(e => e.Value).HasColumnName("email");
                }
            );
            x.OwnsOne(
                p => p.Name,
                p =>
                {
                    p.Property<long>("name_suffix_id").HasColumnName("name_suffix_id");
                    p.Property(pp => pp.First).HasColumnName("FirstName");
                    p.Property(pp => pp.Last).HasColumnName("LastName");
                    p.HasOne(pp => pp.Suffix).WithMany().HasForeignKey("name_suffix_id");
                }
            );
            x.HasOne(p => p.FavoriteCourse).WithMany();
            x.HasMany(p => p.Enrollments)
                .WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });
        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("CourseID");
            x.Property(p => p.Name);
        });
        modelBuilder.Entity<Enrollment>(x =>
        {
            x.ToTable("Enrollment").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("EnrollmentID");
            x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
            x.HasOne(p => p.Course).WithMany();
            x.Property(p => p.Grade);
        });
    }

    public override int SaveChanges()
    {
        int result = base.SaveChanges();

        IEnumerable<Entity> entities = ChangeTracker.Entries<Entity>().Select(x => x.Entity);
        foreach (Entity entity in entities)
        {
            _eventDispatcher.Dispatch(entity.PopDomainEvents());
        }

        return result;
    }
}
