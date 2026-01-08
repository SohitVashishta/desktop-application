using System.Windows;
using System.Windows.Input;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class TeacherAddEditViewModel
    {
        private readonly TeacherService _service = new();
        private readonly Window _window;
        private readonly int _teacherId;

        public string Title => _teacherId == 0 ? "Add Teacher" : "Edit Teacher";

        public string Name { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Email { get; set; } = "";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // ADD
        public TeacherAddEditViewModel(Window window)
        {
            _window = window;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        // EDIT
        public TeacherAddEditViewModel(Window window, Teacher teacher)
            : this(window)
        {
            _teacherId = teacher.TeacherId;
            Name = teacher.FirstName+" "+teacher.LastName;
            Subject = teacher.Subject;
            Email = teacher.Email;
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Name is required");
                return;
            }

            var teacher = new Teacher
            {
                TeacherId = _teacherId,
                Name = Name,
                Subject = Subject,
                Email = Email
            };

            if (_teacherId == 0)
                _service.AddTeacher(teacher);
            else
                _service.UpdateTeacher(teacher);

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
