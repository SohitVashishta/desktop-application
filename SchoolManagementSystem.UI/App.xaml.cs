using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Auth;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Auth;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.ViewModels.Admin;
using SchoolManagementSystem.UI.UI.ViewModels.Attendances;
using SchoolManagementSystem.UI.UI.ViewModels.Dashboard;
using SchoolManagementSystem.UI.UI.ViewModels.FeeManagement;
using SchoolManagementSystem.UI.UI.ViewModels.Import;
using SchoolManagementSystem.UI.UI.ViewModels.Library;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic;
using SchoolManagementSystem.UI.UI.Views.FeeManagement;
using System.Configuration;
using System.Windows;

namespace SchoolManagementSystem.UI
{
    public partial class App : Application
    {
        // ✅ THIS IS WHAT YOU ARE MISSING
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            Services = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var cs = ConfigurationManager.ConnectionStrings["SchoolDb"];

            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'SchoolDb' is missing in App.config");
            }

            // ✅ DB CONTEXT (ONLY ONCE)
            services.AddDbContextFactory<SchoolDbContext>(options =>
     options.UseSqlServer(cs.ConnectionString));


            // ================= REPOSITORIES =================
           
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<ICommunicationRepository, CommunicationRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IFinanceRepository, FinanceRepository>();
            services.AddScoped<IHrRepository, HrRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ISystemRepository, SystemRepository>();
            services.AddScoped<ITimetableRepository, TimetableRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IClassRepository, ClassRepository>(); 
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IClassSectionRepository, ClassSectionRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddScoped<IStudentAdmissionRepository, StudentAdmissionRepository>();
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookTransactionRepository, BookTransactionRepository>();





            // ================= SERVICES =================
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IHrService, HrService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ISystemService, SystemService>();
            services.AddScoped<ITimetableService, TimetableService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IAcademicYearService, AcademicYearService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IClassSectionService, ClassSectionService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IStudentAdmissionService, StudentAdmissionService>();
            services.AddScoped<IFeeService, FeeService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookTransactionService, BookTransactionService>();

            // ================= VIEWMODELS =================
            services.AddTransient<UserManagementViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddScoped<DashboardViewModel>();
            services.AddScoped<TeacherViewModel>();
            services.AddTransient<TeacherAddEditViewModel>();
            services.AddTransient<StudentAddEditViewModel>();
            services.AddTransient<ImportStudentsViewModel>();
            services.AddTransient<ImportTeachersViewModel>();
            services.AddTransient<MarkAttendanceViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AssignRolesViewModel>();
            services.AddTransient<ResetPasswordsViewModel>();
            services.AddTransient<ControlPortalAccessViewModel>();
            services.AddTransient<ClassMasterViewModel>();
            services.AddTransient<AddEditClassDialogViewModel>();
            services.AddTransient<SubjectMasterViewModel>();
            services.AddTransient<AcademicYearViewModel>();
            services.AddTransient<SectionMasterViewModel>();
            services.AddTransient<ClassSectionMappingViewModel>();
            services.AddTransient<StudentListViewModel>();
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<StudentAdmissionViewModel>();
            services.AddTransient<FeeManagementViewModel>();
            services.AddTransient<FeeConcessionViewModel>();
            services.AddTransient<AddNewFeeEntryViewModel>();
            services.AddTransient<AddNewFeeEntry>();
            services.AddTransient<FeeStructureViewModel>();
            services.AddTransient<ReceiptViewModel>();
            services.AddTransient<StudentFeeAssignmentViewModel>();
            services.AddTransient<StudentFeeAssignViewModel>();
            services.AddTransient<BookInventoryViewModel>();
            services.AddTransient<IssueReturnHistoryViewModel>();

        }

    }
}
