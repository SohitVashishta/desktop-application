using System;
using System.Windows;
using System.Windows.Input;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class StudentAddEditViewModel
    {
        private readonly StudentService _service = new();
        private readonly Window _window;
        private readonly int _studentId;

        public string Title => _studentId == 0 ? "Add Student" : "Edit Student";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // ADD
        public StudentAddEditViewModel(Window window)
        {
            _window = window;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        // EDIT
        public StudentAddEditViewModel(Window window, Student student)
            : this(window)
        {
            _studentId = student.StudentId;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            DateOfBirth = student.DateOfBirth;
            EnrollmentDate = student.EnrollmentDate;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("First Name is required");
                return;
            }

            var student = new Student
            {
                StudentId = _studentId,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                EnrollmentDate = EnrollmentDate
            };

            if (_studentId == 0)
                _service.AddStudent(student);
            else
                _service.UpdateStudent(student);

            _window.DialogResult = true;
            _window.Close();
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}
