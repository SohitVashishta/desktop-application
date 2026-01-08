using System;
using System.Collections.Generic;
using System.Linq;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class AttendanceRepository
    {
        // =============================
        // GET BY DATE
        // =============================
        public List<Attendance> GetByDate(DateTime date)
        {
            using var ctx = new SchoolDbContext();
            return ctx.Attendances
                      .Where(a => a.AttendanceDate == date.Date)
                      .ToList();
        }

        // =============================
        // UPSERT SINGLE RECORD
        // =============================
        public void Upsert(int studentId, DateTime date, bool isPresent)
        {
            using var ctx = new SchoolDbContext();

            var record = ctx.Attendances.FirstOrDefault(a =>
                a.StudentId == studentId &&
                a.AttendanceDate == date.Date);

            if (record != null)
            {
                record.IsPresent = isPresent;
            }
            else
            {
                ctx.Attendances.Add(new Attendance
                {
                    StudentId = studentId,
                    AttendanceDate = date.Date,
                    IsPresent = isPresent
                });
            }

            ctx.SaveChanges();
        }
        // =========================================
        // DASHBOARD CALCULATION
        // =========================================
        public int GetAttendancePercentageByDate(DateTime date)
        {
            using var ctx = new SchoolDbContext();

            var total = ctx.Attendances
                           .Count(a => a.AttendanceDate == date.Date);

            if (total == 0)
                return 0;

            var present = ctx.Attendances
                             .Count(a => a.AttendanceDate == date.Date && a.IsPresent);

            return (int)((present * 100.0) / total);
        }


        // =============================
        // BULK UPSERT (FAST & SAFE)
        // =============================
        public void UpsertBulk(DateTime date, List<Attendance> records)
        {
            using var ctx = new SchoolDbContext();

            foreach (var item in records)
            {
                var existing = ctx.Attendances.FirstOrDefault(a =>
                    a.StudentId == item.StudentId &&
                    a.AttendanceDate == date.Date);

                if (existing != null)
                {
                    existing.IsPresent = item.IsPresent;
                }
                else
                {
                    ctx.Attendances.Add(new Attendance
                    {
                        StudentId = item.StudentId,
                        AttendanceDate = date.Date,
                        IsPresent = item.IsPresent
                    });
                }
            }

            ctx.SaveChanges();
        }
    }
}
