using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.Models.Models.SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IStudentService
    {
        Task<List<StudentModel>> GetAllAsync();
        Task<StudentProfileVM> GetProfileAsync(int studentId);

        Task AddAsync(StudentProfileVM profile);
        Task UpdateAsync(StudentProfileVM profile);

        Task ToggleStatusAsync(int studentId);

        Task PromoteAsync(int studentId,
                          int academicYearId,
                          int classId,
                          int sectionId);
        Task BulkUploadAsync(List<StudentModel> students);
        Task SaveAdmissionAsync(StudentProfileVM model);
        Task UpdateAdmissionAsync(StudentAdmissionModel model);
        Task<List<StudentModel>> GetByClassAsync(int academicYearId, int classId);
        Task AssignFeeAsync(int studentId,int academicYearId,decimal paidAmount, string paymentMode, DateTime paymentDate );
        Task<StudentFeeAssignmentModel> GetStudentFeeAssignmentAsync(int studentId);
    }

}
