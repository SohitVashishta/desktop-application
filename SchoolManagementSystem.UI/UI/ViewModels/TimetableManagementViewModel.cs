using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class TimetableManagementViewModel
    {
        private readonly ITimetableService _service = new TimetableService();

        public ObservableCollection<TimetableDto> Timetable { get; } = new();

        // Form Fields
        public string Class { get; set; }
        public string Section { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string Day { get; set; }
        public string TimeSlot { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }

        public TimetableManagementViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddCommand = new RelayCommand(async () => await AddAsync());
        }

        private async Task LoadAsync()
        {
            Timetable.Clear();
            foreach (var t in await _service.GetTimetableAsync())
                Timetable.Add(t);
        }

        private async Task AddAsync()
        {
            var entry = new TimetableDto
            {
                Class = Class,
                Section = Section,
                Subject = Subject,
                Teacher = Teacher,
                Day = Day,
                TimeSlot = TimeSlot
            };

            bool success = await _service.CreateTimetableAsync(entry);
            if (success)
                Timetable.Add(entry);
            // else → conflict (hook dialog later)
        }
    }
}
