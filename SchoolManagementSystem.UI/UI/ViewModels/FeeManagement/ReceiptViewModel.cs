using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

public class ReceiptViewModel : NotifyPropertyChangedBase
{
    private readonly IFeeService _feeService;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;
    private readonly IAcademicYearService _academicYearService;

    public ReceiptViewModel(
        IFeeService feeService,
        IStudentService studentService,
        IClassService classService,
        IAcademicYearService academicYearService)
    {
        _feeService = feeService;
        _studentService = studentService;
        _classService = classService;
        _academicYearService = academicYearService;

        AcademicYears = new ObservableCollection<AcademicYearModel>();
        Classes = new ObservableCollection<ClassModel>();
        Students = new ObservableCollection<StudentModel>();
        FeeDetails = new ObservableCollection<StudentFeeAssignmentDetailModel>();
        ReceiptHistory = new ObservableCollection<FeeReceiptModel>();

        PaymentDate = DateTime.Today;

        FeeDetails.CollectionChanged += (_, __) =>
        {
            OnPropertyChanged(nameof(NetAmount));
            OnPropertyChanged(nameof(BalanceAmount));
        };

        LoadStudentCommand = new RelayCommand(async () => await LoadStudentsAsync());
        GenerateReceiptCommand = new RelayCommand(async () => await GenerateReceiptAsync());
        DownloadPdfCommand = new RelayCommand<FeeReceiptModel>(DownloadPdf);
        PrintCommand = new RelayCommand<FeeReceiptModel>(PrintReceipt);

        _ = LoadMastersAsync();
    }

    /* ================= MASTER ================= */

    public ObservableCollection<AcademicYearModel> AcademicYears { get; }
    public ObservableCollection<ClassModel> Classes { get; }
    public ObservableCollection<StudentModel> Students { get; }

    private async Task LoadMastersAsync()
    {
        AcademicYears.Clear();
        Classes.Clear();

        foreach (var y in await _academicYearService.GetAllAsync())
            AcademicYears.Add(y);

        foreach (var c in await _classService.GetClassesAsync())
            Classes.Add(c);
    }

    /* ================= SELECTIONS ================= */

    private AcademicYearModel _selectedAcademicYear;
    public AcademicYearModel SelectedAcademicYear
    {
        get => _selectedAcademicYear;
        set
        {
            _selectedAcademicYear = value;
            OnPropertyChanged(nameof(SelectedAcademicYear));
            _ = TryLoadStudentsAsync();
        }
    }

    private ClassModel _selectedClass;
    public ClassModel SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
            _ = TryLoadStudentsAsync();
        }
    }

    private StudentModel _selectedStudent;
    public StudentModel SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged(nameof(SelectedStudent));
            _ = LoadStudentFeesAsync();
        }
    }

    /* ================= DATA ================= */

    public ObservableCollection<StudentFeeAssignmentDetailModel> FeeDetails { get; }
    public ObservableCollection<FeeReceiptModel> ReceiptHistory { get; }

    public decimal NetAmount => FeeDetails.Sum(x => x.NetAmount);

    /* ================= PAYMENT ================= */

    private decimal _paidAmount;
    public decimal PaidAmount
    {
        get => _paidAmount;
        set
        {
            _paidAmount = value;
            OnPropertyChanged(nameof(PaidAmount));
            OnPropertyChanged(nameof(BalanceAmount));
        }
    }

    public string SelectedPaymentMode { get; set; }
    public DateTime PaymentDate { get; set; }

    public decimal BalanceAmount => Math.Max(0, NetAmount - PaidAmount);

    /* ================= COMMANDS ================= */

    public ICommand LoadStudentCommand { get; }
    public ICommand GenerateReceiptCommand { get; }
    public ICommand DownloadPdfCommand { get; }
    public ICommand PrintCommand { get; }

    /* ================= LOAD ================= */

    private async Task TryLoadStudentsAsync()
    {
        if (SelectedAcademicYear == null || SelectedClass == null)
            return;

        await LoadStudentsAsync();
    }

    private async Task LoadStudentsAsync()
    {
        Students.Clear();
        FeeDetails.Clear();
        ReceiptHistory.Clear();

        var list = await _studentService.GetByClassAsync(
            SelectedAcademicYear.AcademicYearId,
            SelectedClass.ClassId);

        foreach (var s in list)
            Students.Add(s);

        if (Students.Any())
            SelectedStudent = Students.First();
    }

    private async Task LoadStudentFeesAsync()
    {
        if (SelectedStudent == null) return;

        FeeDetails.Clear();
        ReceiptHistory.Clear();

        var assignment = await _studentService
            .GetStudentFeeAssignmentAsync(SelectedStudent.StudentId);

        if (assignment != null)
        {
            foreach (var d in assignment.Details)
            {
                FeeDetails.Add(d);
                d.PropertyChanged += FeeDetail_PropertyChanged;
            }
        }

        var receipts = await _feeService.GetReceiptsAsync(SelectedStudent.StudentId);
        foreach (var r in receipts)
            ReceiptHistory.Add(r);
    }

    private void FeeDetail_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StudentFeeAssignmentDetailModel.NetAmount))
        {
            OnPropertyChanged(nameof(NetAmount));
            OnPropertyChanged(nameof(BalanceAmount));
        }
    }

    /* ================= RECEIPT ================= */

    private async Task GenerateReceiptAsync()
    {
        if (SelectedStudent == null || PaidAmount <= 0)
        {
            MessageBox.Show("Invalid payment");
            return;
        }

        var receipt = new FeeReceiptModel
        {
            ReceiptNo = GenerateReceiptNo(),
            StudentId = SelectedStudent.StudentId,
            StudentName = SelectedStudent.StudentName,
            ClassName = SelectedClass.ClassName,
            PaidAmount = PaidAmount,
            NetFees = NetAmount,
            BalanceAmount = BalanceAmount,
            PaymentMode = SelectedPaymentMode,
            PaymentDate = PaymentDate
        };

        await _feeService.SaveReceiptAsync(receipt);

        ReceiptHistory.Insert(0, receipt);
        PaidAmount = 0;
    }

    /* ================= PDF / PRINT ================= */

    private void DownloadPdf(FeeReceiptModel receipt)
    {
        if (receipt == null) return;

        var path = $@"D:\Receipts\{receipt.ReceiptNo}.pdf";
        FeeReceiptPdfGenerator.Generate(receipt, path);
        MessageBox.Show("PDF generated.");
    }

    private void PrintReceipt(FeeReceiptModel receipt)
    {
        if (receipt == null) return;

        var path = $@"D:\Receipts\{receipt.ReceiptNo}.pdf";
        if (!File.Exists(path))
            FeeReceiptPdfGenerator.Generate(receipt, path);

        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }

    private static string GenerateReceiptNo()
        => $"RCPT-{DateTime.Now:yyyyMMddHHmmss}";
}
