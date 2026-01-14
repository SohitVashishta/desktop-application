using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class TeacherAddEditViewModel : BaseViewModel
    {
        private readonly ITeacherService _teacherService;

        private int _teacherId;

        public string Title => _teacherId == 0 ? "Add Teacher" : "Edit Teacher";

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _subject = string.Empty;
        public string Subject
        {
            get => _subject;
            set { _subject = value; OnPropertyChanged(); }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // ✅ Event to notify View
        public event Action<bool>? CloseRequested;

        // ADD
        public TeacherAddEditViewModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(false));
        }

        // EDIT
        public TeacherAddEditViewModel(
            ITeacherService teacherService,
            Teacher teacher) : this(teacherService)
        {
            _teacherId = teacher.TeacherId;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            Subject = teacher.Subject;
            Email = teacher.Email;

            OnPropertyChanged(nameof(Title));
        }

        // =============================
        // SAVE (ASYNC)
        // =============================
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("First name is required");
                return;
            }

            var teacher = new Teacher
            {
                TeacherId = _teacherId,
                FirstName = FirstName,
                LastName = LastName,
                Subject = Subject,
                Email = Email
            };

            if (_teacherId == 0)
                await _teacherService.AddTeacherAsync(teacher);
            else
                await _teacherService.UpdateTeacherAsync(teacher);

            CloseRequested?.Invoke(true);
        }
    }
}
