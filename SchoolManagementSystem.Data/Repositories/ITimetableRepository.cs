using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface ITimetableRepository
    {
        Task<List<TimetableDto>> GetTimetableAsync();
        Task AddTimetableAsync(TimetableDto timetable);
        Task<bool> HasConflictAsync(TimetableDto timetable);
    }
}
