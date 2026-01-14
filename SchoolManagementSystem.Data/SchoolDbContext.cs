using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;

namespace SchoolManagementSystem.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<NotificationDto> Notifications { get; set; }
        public DbSet<FeeInvoice> FeeInvoices { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
