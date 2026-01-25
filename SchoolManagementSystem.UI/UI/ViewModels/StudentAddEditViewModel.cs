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
    public class StudentAddEditViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;
        private int _studentId;

        // ================= UI PROPERTIES =================

        public string Title => _studentId == 0 ? "Add Student" : "Edit Student";

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

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }

        private DateTime _enrollmentDate = DateTime.Today;
        public DateTime EnrollmentDate
        {
            get => _enrollmentDate;
            set { _enrollmentDate = value; OnPropertyChanged(); }
        }

        // ================= COMMANDS =================

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Notify View to close
        public event Action<bool>? CloseRequested;

        // ================= CONSTRUCTORS =================

        // ADD
        public StudentAddEditViewModel(IStudentService studentService)
        {
            _studentService = studentService;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(false));
        }

        // EDIT
        public StudentAddEditViewModel(
            IStudentService studentService,
            Student student) : this(studentService)
        {
            _studentId = student.StudentId;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            DateOfBirth = student.DateOfBirth;
            EnrollmentDate = student.EnrollmentDate;

            OnPropertyChanged(nameof(Title));
        }

        // ================= SAVE =================

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("First Name is required");
                return;
            }

            if (DateOfBirth == null)
            {
                MessageBox.Show("Date of Birth is required");
                return;
            }

            var student = new StudentModel
            {
                StudentId = _studentId,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                DateOfBirth = DateOfBirth.Value,
                EnrollmentDate = EnrollmentDate
            };
StudentProfileVM studentProfileVM = new StudentProfileVM();
            studentProfileVM.Student=student;
            if (_studentId == 0)
                await _studentService.AddAsync(studentProfileVM);
            else
                await _studentService.UpdateAsync(studentProfileVM);

            CloseRequested?.Invoke(true);
        }
    }
}
