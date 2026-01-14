using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class StudentManagementViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;

        public ObservableCollection<Student> Students { get; } = new();

        // ✅ Constructor Injection
        public StudentManagementViewModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task LoadAsync()
        {
            Students.Clear();

            var students = await _studentService.GetStudentsAsync();
            foreach (var s in students)
                Students.Add(s);
        }
    }
}
