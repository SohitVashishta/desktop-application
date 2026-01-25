using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic
{
    public class AddEditClassDialogViewModel : BaseViewModel
    {
        private readonly IGradeService _gradeService;
        private readonly ITeacherService _teacherService;
        private readonly ClassModel _editModel;

        /* ================= MODE ================= */

        public bool IsEditMode => _editModel != null;

        public string DialogTitle =>
            IsEditMode ? "Edit Class" : "Add Class";

        /* ================= DROPDOWNS ================= */

        public ObservableCollection<GradeModel> Grades { get; } = new();
        public ObservableCollection<TeacherMaster> Teachers { get; } = new();

        /* ================= FIELDS ================= */

        private string _className;
        public string ClassName
        {
            get => _className;
            set { _className = value; OnPropertyChanged(); Validate(); }
        }

        private string _section;
        public string Section
        {
            get => _section;
            set { _section = value; OnPropertyChanged(); Validate(); }
        }

        private int _selectedGradeId;
        public int SelectedGradeId
        {
            get => _selectedGradeId;
            set { _selectedGradeId = value; OnPropertyChanged(); Validate(); }
        }

        private int? _selectedTeacherId;
        public int? SelectedTeacherId
        {
            get => _selectedTeacherId;
            set { _selectedTeacherId = value; OnPropertyChanged(); }
        }

        /* ================= VALIDATION ================= */

        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            private set { _canSave = value; OnPropertyChanged(); }
        }

        private void Validate()
        {
            CanSave =
                !string.IsNullOrWhiteSpace(ClassName) &&
                !string.IsNullOrWhiteSpace(Section) &&
                SelectedGradeId > 0;
        }

        /* ================= CONSTRUCTORS ================= */

        // ADD MODE
        public AddEditClassDialogViewModel(
            IGradeService gradeService,
            ITeacherService teacherService)
        {
            _gradeService = gradeService;
            _teacherService = teacherService;
        }

        // EDIT MODE
        public AddEditClassDialogViewModel(
            IGradeService gradeService,
            ITeacherService teacherService,
            ClassModel editModel) : this(gradeService, teacherService)
        {
            _editModel = editModel;

            ClassName = editModel.ClassName;
            Section = editModel.Section;
            SelectedGradeId = editModel.GradeId;
            SelectedTeacherId = editModel.LeadTeacherId;
        }

        /* ================= LOAD DROPDOWNS ================= */

        public async Task LoadDropdownsAsync()
        {
            Grades.Clear();
            Teachers.Clear();

            var grades = await _gradeService.GetActiveGradesAsync();
            foreach (var g in grades)
                Grades.Add(g);

            var teachers = await _teacherService.GetTeachersAsync();
            foreach (var t in teachers)
                Teachers.Add(t);

            // Ensure selected values still exist (Edit mode safety)
            if (IsEditMode)
            {
                if (!Grades.Any(g => g.GradeId == SelectedGradeId))
                    SelectedGradeId = 0;

                if (!Teachers.Any(t => t.TeacherId == SelectedTeacherId))
                    SelectedTeacherId = null;
            }

            Validate();
        }

        /* ================= BUILD RESULT ================= */

        public ClassModel BuildModel()
        {
            return new ClassModel
            {
                ClassId = _editModel?.ClassId ?? 0,

                ClassName = ClassName.Trim(),
                Section = Section.Trim(),
                GradeId = SelectedGradeId,
                LeadTeacherId = SelectedTeacherId,

                CreatedOn = _editModel == null ? DateTime.Now : _editModel.CreatedOn,
                CreatedBy = _editModel == null ? "Admin" : _editModel.CreatedBy,

               // UpdatedOn = _editModel != null ? DateTime.Now : null,
               // UpdatedBy = _editModel != null ? "Admin" : null
            };
        }
    }
}
