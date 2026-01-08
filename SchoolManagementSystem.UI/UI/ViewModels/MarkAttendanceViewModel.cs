using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Attendances
{
    public class MarkAttendanceViewModel
    {
        private readonly AttendanceService _attendanceService = new();
        private readonly StudentService _studentService = new();

        public DateTime SelectedDate { get; set; } = DateTime.Today;

        // UI collection
        public ObservableCollection<StudentAttendanceViewModel> Students { get; set; }

        public ICommand SaveCommand { get; }

        public MarkAttendanceViewModel()
        {
            LoadStudents();
            SaveCommand = new RelayCommand(Save);
        }

        // =========================================
        // LOAD STUDENTS (UI ONLY)
        // =========================================
        private void LoadStudents()
        {
            var students = _studentService.GetStudents();

            Students = new ObservableCollection<StudentAttendanceViewModel>(
                students.Select(s => new StudentAttendanceViewModel
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    IsPresent = true // default checked
                })
            );
        }

        // =========================================
        // SAVE ATTENDANCE
        // =========================================
        private void Save()
        {
            var records = Students.Select(s => new Attendance
            {
                StudentId = s.StudentId,
                AttendanceDate = SelectedDate.Date,
                IsPresent = s.IsPresent
            }).ToList();

            _attendanceService.SaveAttendanceBulk(SelectedDate, records);

            MessageBox.Show("Attendance saved successfully");
        }
    }
}
