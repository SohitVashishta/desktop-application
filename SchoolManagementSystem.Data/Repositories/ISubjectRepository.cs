using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<SubjectModel>> GetAllAsync();
        Task AddAsync(SubjectModel model);
        Task UpdateAsync(SubjectModel model);
        Task DeleteAsync(int subjectId);
    }

}
