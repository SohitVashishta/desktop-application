using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IExamService
    {
        Task<List<ExamDto>> GetExamsAsync();
        Task CreateExamAsync(ExamDto exam);
        Task ApproveMarksAsync(int examId);
    }
}
