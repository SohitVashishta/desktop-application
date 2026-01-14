using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class ExamRepository : IExamRepository
    {
        public Task<List<ExamDto>> GetExamsAsync()
        {
            return Task.FromResult(new List<ExamDto>
            {
                new ExamDto
                {
                    ExamId = 1,
                    ExamName = "Mid Term",
                    Class = "10",
                    Subject = "Math",
                    ExamDate = "15-Aug-2026",
                    MaxMarks = 100,
                    IsApproved = false
                }
            });
        }

        public Task AddExamAsync(ExamDto exam)
        {
            // INSERT INTO Exams...
            return Task.CompletedTask;
        }

        public Task ApproveMarksAsync(int examId)
        {
            // UPDATE Exams SET IsApproved = 1
            return Task.CompletedTask;
        }
    }
}
