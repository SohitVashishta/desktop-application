using System;
using System.Collections.Generic;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class AttendanceService
    {
        private readonly AttendanceRepository _repo = new AttendanceRepository();

        // Get attendance for a date (optional, for reports/edit)
        public List<Attendance> GetByDate(DateTime date)
            => _repo.GetByDate(date);

        // Save / Update attendance (UPSERT)
        public void SaveAttendance(int studentId, DateTime date, bool isPresent)
            => _repo.Upsert(studentId, date, isPresent);

        // Bulk save (used by MarkAttendanceViewModel)
        public void SaveAttendanceBulk(DateTime date, List<Attendance> records)
            => _repo.UpsertBulk(date, records);

        // =========================================
        // DASHBOARD: TODAY ATTENDANCE %
        // =========================================
        public int GetTodayAttendancePercentage()
        {
            return _repo.GetAttendancePercentageByDate(DateTime.Today);
        }

        
    }
}
