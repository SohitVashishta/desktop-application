using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IImportRepository
    {
        Task ImportStudentsAsync(List<Student> students);
        Task ImportTeachersAsync(List<Teacher> teachers);
    }
}
