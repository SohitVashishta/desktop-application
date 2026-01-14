using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using System.Linq;

namespace SchoolManagementSystem.Data.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly SchoolDbContext _context;

        // ✅ Inject DbContext
        public AttendanceRepository(SchoolDbContext context)
        {
            _context = context;
        }

        // =============================
        // GET BY DATE
        // =============================
        public async Task<List<Attendance>> GetByDateAsync(DateTime date)
        {
            var targetDate = date.Date;

            return await _context.Attendances
                .AsNoTracking()
                .Where(a => a.AttendanceDate == targetDate)
                .ToListAsync();
        }

        // =============================
        // UPSERT SINGLE RECORD
        // =============================
        public async Task UpsertAsync(int studentId, DateTime date, bool isPresent)
        {
            var targetDate = date.Date;

            var record = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.AttendanceDate == targetDate);

            if (record != null)
            {
                record.IsPresent = isPresent;
            }
            else
            {
                await _context.Attendances.AddAsync(new Attendance
                {
                    StudentId = studentId,
                    AttendanceDate = targetDate,
                    IsPresent = isPresent
                });
            }

            await _context.SaveChangesAsync();
        }

        // =============================
        // BULK UPSERT (STANDARD & SAFE)
        // =============================
        public async Task UpsertBulkAsync(DateTime date, List<Attendance> records)
        {
            var targetDate = date.Date;

            var studentIds = records.Select(r => r.StudentId).ToList();

            var existingRecords = await _context.Attendances
                .Where(a => a.AttendanceDate == targetDate &&
                            studentIds.Contains(a.StudentId))
                .ToListAsync();

            foreach (var item in records)
            {
                var existing = existingRecords
                    .FirstOrDefault(x => x.StudentId == item.StudentId);

                if (existing != null)
                {
                    existing.IsPresent = item.IsPresent;
                }
                else
                {
                    await _context.Attendances.AddAsync(new Attendance
                    {
                        StudentId = item.StudentId,
                        AttendanceDate = targetDate,
                        IsPresent = item.IsPresent
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        // =============================
        // DASHBOARD CALCULATION (STANDARD)
        // =============================
        public async Task<int> GetAttendancePercentageByDateAsync(DateTime date)
        {
            var targetDate = date.Date;

            var total = await _context.Attendances
                .CountAsync(a => a.AttendanceDate == targetDate);

            if (total == 0)
                return 0;

            var present = await _context.Attendances
                .CountAsync(a => a.AttendanceDate == targetDate && a.IsPresent);

            return (int)((present * 100.0) / total);
        }
    }
}
