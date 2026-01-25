using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Student
{
    public class AddStudentViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;

        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<SectionModel> Sections { get; } = new();

        public string AdmissionNo { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public ClassModel SelectedClass { get; set; }
        public SectionModel SelectedSection { get; set; }

        public ICommand SaveCommand { get; }

        public AddStudentViewModel(
            IStudentService studentService,
            IClassService classService,
            ISectionService sectionService)
        {
            _studentService = studentService;
            _classService = classService;
            _sectionService = sectionService;

            SaveCommand = new RelayCommand(async () => await SaveAsync());

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Classes.Clear();
            foreach (var c in await _classService.GetClassesAsync())
                Classes.Add(c);

            Sections.Clear();
            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);
        }

        private async Task SaveAsync()
        {
            var model = new StudentModel
            {
                AdmissionNo = AdmissionNo,
                StudentName = StudentName,
                Gender = Gender,
                ClassId = SelectedClass?.ClassId ?? 0,
                SectionId = SelectedSection?.SectionId ?? 0,
                DateOfBirth = DateOfBirth
            };
            StudentProfileVM studentProfileVM = new StudentProfileVM();
            studentProfileVM.Student = model;
            await _studentService.AddAsync(studentProfileVM);

            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }

}
