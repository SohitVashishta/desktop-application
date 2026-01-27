using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement
{
    public class StudentAdmissionViewModel : ValidatableViewModel
    {
        #region SERVICES

        private readonly IStudentService _studentService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;

        #endregion

        #region MODE

        private readonly AdmissionMode _mode;
        public bool IsReadOnly => _mode == AdmissionMode.View;
        public bool CanSave => _mode != AdmissionMode.View && !HasErrors;

        #endregion

        #region PROFILE

        public StudentProfileVM Profile { get; private set; }

        #endregion

        #region STEPPER

        private int _currentStep;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }


        public ICommand NextStepCommand { get; }
        public ICommand PrevStepCommand { get; }

        private void NextStep()
        {
            ValidateCurrentStep();
            if (HasErrors) return;

            if (CurrentStep < 4)
                CurrentStep++;
        }

        private void PrevStep()
        {
            if (CurrentStep > 0)
                CurrentStep--;
        }

        private void ValidateCurrentStep()
        {
            // 🔹 Clear only current step errors
            switch (CurrentStep)
            {
                case 0: // Student
                    ClearErrors(nameof(Profile.Student.FirstName));
                    ClearErrors(nameof(Profile.Student.LastName));
                    ClearErrors(nameof(Profile.Student.DateOfBirth));

                    if (string.IsNullOrWhiteSpace(Profile.Student.FirstName))
                        AddError(nameof(Profile.Student.FirstName), "First name required");

                    if (string.IsNullOrWhiteSpace(Profile.Student.LastName))
                        AddError(nameof(Profile.Student.LastName), "Last name required");

                    if (Profile.Student.DateOfBirth == null)
                        AddError(nameof(Profile.Student.DateOfBirth), "DOB required");
                    break;

                case 1: // Academic
                    ClearErrors(nameof(Profile.Student.ClassId));
                    ClearErrors(nameof(Profile.Student.SectionId));

                    if (Profile.Student.ClassId <= 0)
                        AddError(nameof(Profile.Student.ClassId), "Class required");

                    if (Profile.Student.SectionId <= 0)
                        AddError(nameof(Profile.Student.SectionId), "Section required");
                    break;

                case 2: // Parents
                    ClearErrors(nameof(Profile.Parent.FatherName));

                    if (string.IsNullOrWhiteSpace(Profile.Parent.FatherName))
                        AddError(nameof(Profile.Parent.FatherName), "Father name required");
                    break;

                case 4: // Fees
                    ClearErrors(nameof(TotalFees));
                    ClearErrors(nameof(IsDeclarationAccepted));

                    if (TotalFees <= 0)
                        AddError(nameof(TotalFees), "Total fees required");

                    if (!IsDeclarationAccepted)
                        AddError(nameof(IsDeclarationAccepted), "Declaration required");
                    break;
            }
        }


        #endregion

        #region DROPDOWNS

        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();
        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<SectionModel> Sections { get; } = new();

        public ObservableCollection<string> GenderList { get; } = new() { "Male", "Female", "Other" };
        public ObservableCollection<string> CategoryList { get; } = new() { "General", "OBC", "SC", "ST" };
        public ObservableCollection<string> ReligionList { get; } = new() { "Hindu", "Muslim", "Christian", "Sikh", "Other" };
        public ObservableCollection<string> MotherTongueList { get; } = new() { "Hindi", "English", "Other" };
        public ObservableCollection<string> YesNoList { get; } = new() { "Yes", "No" };

        public ObservableCollection<string> AdmissionTypeList { get; } =
            new() { "New Admission", "Transfer" };

        public ObservableCollection<string> PaymentModes { get; } =
            new() { "Cash", "UPI", "Card", "Bank Transfer" };

        #endregion

        #region SELECTED LOOKUPS

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set
            {
                _selectedAcademicYear = value;
                Profile.Student.AcademicYearId = value?.AcademicYearId ?? 0;
                OnPropertyChanged(nameof(SelectedAcademicYear));

            }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                Profile.Student.ClassId = value?.ClassId ?? 0;
                OnPropertyChanged(nameof(SelectedClass));

            }
        }

        private SectionModel _selectedSection;
        public SectionModel SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection = value;
                Profile.Student.SectionId = value?.SectionId ?? 0;
                OnPropertyChanged(nameof(SelectedSection));

            }
        }

        #endregion

        #region FEES

        private decimal _totalFees;
        public decimal TotalFees
        {
            get => _totalFees;
            set
            {
                _totalFees = value;
                OnPropertyChanged(nameof(TotalFees));

                OnPropertyChanged(nameof(BalanceFees));
            }
        }

        private decimal _paidFees;
        public decimal PaidFees
        {
            get => _paidFees;
            set
            {
                _paidFees = value;
                OnPropertyChanged(nameof(PaidFees));

                OnPropertyChanged(nameof(BalanceFees));
            }
        }

        public decimal BalanceFees => Math.Max(0, TotalFees - PaidFees);

        public string SelectedPaymentMode { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        private bool _isDeclarationAccepted;
        public bool IsDeclarationAccepted
        {
            get => _isDeclarationAccepted;
            set
            {
                _isDeclarationAccepted = value;
                OnPropertyChanged(nameof(IsDeclarationAccepted));

            }
        }
        private ImageSource _studentPhoto;
        public ImageSource StudentPhoto
        {
            get => _studentPhoto;
            set { _studentPhoto = value; OnPropertyChanged(nameof(StudentPhoto)); }
        }

        public ICommand UploadPhotoCommand => new RelayCommand(() =>
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                StudentPhoto = new BitmapImage(new Uri(dlg.FileName));

                Profile.Documents.Add(new StudentDocumentModel
                {
                    DocumentType = "Photo",
                    FileName = Path.GetFileName(dlg.FileName),
                    FilePath = dlg.FileName
                });
            }
        });
        public ICommand RemoveDocumentCommand => new RelayCommand<StudentDocumentModel>(doc =>
        {
            if (doc != null && Profile.Documents.Contains(doc))
                Profile.Documents.Remove(doc);
        });


        private void UploadDocument(string type)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files|*.pdf|All Files|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                Profile.Documents.Add(new StudentDocumentModel
                {
                    DocumentType = type,
                    FileName = Path.GetFileName(dlg.FileName),
                    FilePath = dlg.FileName
                });
            }
        }

       

        public ICommand UploadTransferCertificateCommand
            => new RelayCommand(() => UploadDocument("TransferCertificate"));

        public ICommand UploadAadhaarCommand
            => new RelayCommand(() => UploadDocument("Aadhaar"));

        public ICommand UploadCasteCertificateCommand
            => new RelayCommand(() => UploadDocument("CasteCertificate"));

        public string FeeValidationMessage { get; private set; }

        #endregion

        #region COMMANDS

        public ICommand SaveCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CancelCommand { get; }
       

        public ICommand UploadBirthCertificateCommand => new RelayCommand(() =>
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|Image Files (*.jpg;*.png)|*.jpg;*.png",
                Title = "Select Birth Certificate"
            };

            if (dialog.ShowDialog() != true)
                return;

            Profile.Documents.Add(new StudentDocumentModel
            {
                DocumentType = "BirthCertificate",
                FileName = System.IO.Path.GetFileName(dialog.FileName),
                FilePath = dialog.FileName
            });
        });


        #endregion

        #region CONSTRUCTORS

        public StudentAdmissionViewModel(
            IStudentService studentService,
            IAcademicYearService academicYearService,
            IClassService classService,
            ISectionService sectionService)
        {
            _studentService = studentService;
            _academicYearService = academicYearService;
            _classService = classService;
            _sectionService = sectionService;

            _mode = AdmissionMode.Add;
            Profile = CreateEmptyProfile();

            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            RefreshCommand = new RelayCommand(async () => await LoadAsync());
            CancelCommand = new RelayCommand(() => { });

            NextStepCommand = new RelayCommand(NextStep);
            PrevStepCommand = new RelayCommand(PrevStep);

            ErrorsChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(CanSave));
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            };

            _ = LoadAsync();
        }

        public StudentAdmissionViewModel(
            IStudentService studentService,
            IAcademicYearService academicYearService,
            IClassService classService,
            ISectionService sectionService,
            StudentProfileVM profile,
            AdmissionMode mode)
            : this(studentService, academicYearService, classService, sectionService)
        {
            _mode = mode;
            Profile = profile ?? CreateEmptyProfile();
        }

        #endregion

        #region LOAD

        private async Task LoadAsync()
        {
            AcademicYears.Clear();
            Classes.Clear();
            Sections.Clear();

            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);

            foreach (var c in await _classService.GetClassesAsync())
                Classes.Add(c);

            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);

            SelectedAcademicYear = AcademicYears.FirstOrDefault(x => x.IsCurrent);

            if (_mode == AdmissionMode.Add)
                GenerateAdmissionNo();
        }

        #endregion

        #region SAVE

        private async Task SaveAsync()
        {
            ValidateCurrentStep();
            if (HasErrors || IsReadOnly) return;

            Profile.Fees.TotalFees = TotalFees;
            Profile.Fees.PaidFees = PaidFees;
            Profile.Fees.PaymentMode = SelectedPaymentMode;
            Profile.Fees.ReceiptNo = ReceiptNo;
            Profile.Fees.PaymentDate = PaymentDate;

            Profile.ClassHistory.Clear();
            Profile.ClassHistory.Add(new StudentClassHistoryModel
            {
                AcademicYearId = Profile.Student.AcademicYearId,
                ClassId = Profile.Student.ClassId,
                SectionId = Profile.Student.SectionId,
                IsCurrent = true
            });

            if (_mode == AdmissionMode.Add)
                await _studentService.SaveAdmissionAsync(Profile);
            else
                await _studentService.UpdateAsync(Profile);
        }

        #endregion

        #region HELPERS

        private static StudentProfileVM CreateEmptyProfile()
        {
            return new StudentProfileVM
            {
                Student = new StudentModel
                {
                    AdmissionDate = DateTime.Today,
                    Nationality = "Indian",
                    IsActive = true
                },
                Parent = new StudentParentModel(),
                Address = new StudentAddressModel(),
                Fees = new StudentFeeModel { PaymentDate = DateTime.Today },
                Documents = new ObservableCollection<StudentDocumentModel>(),
                ClassHistory = new List<StudentClassHistoryModel>()
            };
        }

        private void GenerateAdmissionNo()
        {
            Profile.Student.AdmissionNo = $"ADM-{DateTime.Now:yyyyMMddHHmmss}";
        }

        #endregion
    }

    public enum AdmissionMode
    {
        Add,
        Edit,
        View
    }
}
