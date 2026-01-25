using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Student
{
    public class EditStudentViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;

        public StudentModel Student { get; }

        public string AdmissionNo
        {
            get => Student.AdmissionNo;
            set { Student.AdmissionNo = value; OnPropertyChanged(); }
        }

        public string StudentName
        {
            get => Student.StudentName;
            set { Student.StudentName = value; OnPropertyChanged(); }
        }

        public string ClassName
        {
            get => Student.ClassName;
            set { Student.ClassName = value; OnPropertyChanged(); }
        }

        public string SectionName
        {
            get => Student.SectionName;
            set { Student.SectionName = value; OnPropertyChanged(); }
        }

        public string Gender
        {
            get => Student.Gender;
            set { Student.Gender = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public EditStudentViewModel(IStudentService studentService, StudentModel student)
        {
            _studentService = studentService;
            Student = student;
            StudentProfileVM vm = new StudentProfileVM();
            vm.Student = student;
            SaveCommand = new RelayCommand(async () =>
            {
                await _studentService.UpdateAsync(vm);
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
    }

}
