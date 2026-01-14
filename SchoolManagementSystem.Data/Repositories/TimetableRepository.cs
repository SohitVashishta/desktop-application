using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly List<TimetableDto> _data = new()
        {
            new TimetableDto
            {
                TimetableId = 1,
                Class = "10",
                Section = "A",
                Subject = "Math",
                Teacher = "Mr. Sharma",
                Day = "Monday",
                TimeSlot = "09:00 - 10:00"
            }
        };

        public Task<List<TimetableDto>> GetTimetableAsync()
            => Task.FromResult(_data);

        public Task AddTimetableAsync(TimetableDto timetable)
        {
            _data.Add(timetable);
            return Task.CompletedTask;
        }

        public Task<bool> HasConflictAsync(TimetableDto timetable)
        {
            bool conflict = _data.Any(t =>
                t.Teacher == timetable.Teacher &&
                t.Day == timetable.Day &&
                t.TimeSlot == timetable.TimeSlot);

            return Task.FromResult(conflict);
        }
    }
}
