using SchoolManagementSystem.Business;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        // ✅ Constructor Injection
        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        // =============================
        // GET ATTENDANCE BY DATE
        // =============================
        public async Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date)
        {
            if (date == DateTime.MinValue)
                throw new ArgumentException("Invalid attendance date");

            return await _attendanceRepository.GetByDateAsync(date);
        }

        // =============================
        // MARK SINGLE ATTENDANCE
        // =============================
        public async Task MarkAttendanceAsync(int studentId, DateTime date, bool isPresent)
        {
            if (studentId <= 0)
                throw new ArgumentException("Invalid student");

            if (date == DateTime.MinValue)
                throw new ArgumentException("Invalid date");

            await _attendanceRepository.UpsertAsync(studentId, date, isPresent);
        }

        // =============================
        // MARK BULK ATTENDANCE
        // =============================
        public async Task MarkBulkAttendanceAsync(DateTime date, List<Attendance> records)
        {
            if (records == null || records.Count == 0)
                throw new ArgumentException("Attendance list cannot be empty");

            await _attendanceRepository.UpsertBulkAsync(date, records);
        }

        // =============================
        // DASHBOARD % CALCULATION
        // =============================
        public async Task<int> GetAttendancePercentageAsync(DateTime date)
        {
            return await _attendanceRepository.GetAttendancePercentageByDateAsync(date);
        }
    }
}
