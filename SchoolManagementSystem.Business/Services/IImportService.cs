using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IImportService
    {
        Task ImportStudentsAsync(List<Student> students);
        Task ImportTeachersAsync(List<Teacher> teachers); 
        Task ImportTeachersWithProgressAsync(
        string filePath,
        IProgress<int> progress);

        Task ImportStudentsWithProgressAsync(
                string filePath,
                IProgress<int> progress);

        
    }
}
