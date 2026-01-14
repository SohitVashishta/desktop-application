using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _repository = new TimetableRepository();

        public Task<List<TimetableDto>> GetTimetableAsync()
            => _repository.GetTimetableAsync();

        public async Task<bool> CreateTimetableAsync(TimetableDto timetable)
        {
            if (await _repository.HasConflictAsync(timetable))
                return false;

            await _repository.AddTimetableAsync(timetable);
            return true;
        }
    }
}
