using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.Models.Models.SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IStudentRepository
    {
        Task<List<StudentModel>> GetAllAsync();
        Task<int> AddAsync(StudentModel student);
        Task UpdateAsync(StudentModel student);
        Task ToggleStatusAsync(int studentId);

        Task InsertClassHistoryAsync(StudentClassHistoryModel history);

        // PROMOTION
        Task MarkHistoryNotCurrentAsync(int studentId);
        Task UpdateClassSectionAsync(int studentId, int classId, int sectionId);
        Task<int> AddAdmissionAsync(StudentProfileVM model);
        Task UpdateAdmissionAsync(StudentAdmissionModel model);
    }


}
