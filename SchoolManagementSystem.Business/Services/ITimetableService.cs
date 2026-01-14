using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface ITimetableService
    {
        Task<List<TimetableDto>> GetTimetableAsync();
        Task<bool> CreateTimetableAsync(TimetableDto timetable);
    }
}
