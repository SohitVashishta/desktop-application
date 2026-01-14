using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class ExamManagementViewModel
    {
        private readonly IExamService _service = new ExamService();

        public ObservableCollection<ExamDto> Exams { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand ApproveCommand { get; }

        public ExamManagementViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            ApproveCommand = new RelayCommand<ExamDto>(ApproveMarks);
        }

        private async Task LoadAsync()
        {
            Exams.Clear();
            var data = await _service.GetExamsAsync();
            foreach (var e in data)
                Exams.Add(e);
        }

        private async void ApproveMarks(ExamDto exam)
        {
            if (exam == null || exam.IsApproved) return;

            await _service.ApproveMarksAsync(exam.ExamId);
            exam.IsApproved = true;
        }
    }
}
