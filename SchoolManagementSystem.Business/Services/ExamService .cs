using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _repository = new ExamRepository();

        public Task<List<ExamDto>> GetExamsAsync()
            => _repository.GetExamsAsync();

        public Task CreateExamAsync(ExamDto exam)
            => _repository.AddExamAsync(exam);

        public Task ApproveMarksAsync(int examId)
            => _repository.ApproveMarksAsync(examId);
    }
}
