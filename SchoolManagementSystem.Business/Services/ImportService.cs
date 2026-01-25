using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using System.Data;

namespace SchoolManagementSystem.Business.Services
{
    public class ImportService : IImportService
    {
        private readonly IImportRepository _repo;

        public ImportService(IImportRepository repo)
        {
            _repo = repo;
        }

        public Task ImportStudentsAsync(List<Student> students)
            => _repo.ImportStudentsAsync(students);

        public Task ImportStudentsWithProgressAsync(string filePath, IProgress<int> progress)
        {
            throw new NotImplementedException();
        }

        public Task ImportTeachersAsync(List<TeacherMaster> teachers)
            => _repo.ImportTeachersAsync(teachers);

        public Task ImportTeachersWithProgressAsync(string filePath, IProgress<int> progress)
        {
            throw new NotImplementedException();
        }
    }
}
