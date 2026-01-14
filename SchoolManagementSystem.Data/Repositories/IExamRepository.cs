using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IExamRepository
    {
        Task<List<ExamDto>> GetExamsAsync();
        Task AddExamAsync(ExamDto exam);
        Task ApproveMarksAsync(int examId);
    }
}
