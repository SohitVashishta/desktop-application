using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface ISectionRepository
    {
        Task<List<SectionModel>> GetAllAsync();
        Task AddAsync(SectionModel model);
        Task UpdateAsync(SectionModel model);
    }
}
