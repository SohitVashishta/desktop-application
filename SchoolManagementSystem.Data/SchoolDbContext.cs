using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;

namespace SchoolManagementSystem.Data
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FeeInvoice> FeeInvoices { get; set; }
        public DbSet<User> Users { get; set; } // NEW>


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "Data Source=SOHITVASHISHTHA;Initial Catalog=SchoolDB;Integrated Security=True;Trust Server Certificate=True"
            );
        }
    }

}
