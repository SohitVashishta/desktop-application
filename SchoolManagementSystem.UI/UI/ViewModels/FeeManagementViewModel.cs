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
    public class FeeManagementViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _service = new FeeService();

        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();
        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<FeeHeadModel> FeeHeads { get; } = new();

        public ObservableCollection<FeeStructureDetailModel> FeeDetails { get; } = new();

        public int SelectedAcademicYearId { get; set; }
        public int SelectedClassId { get; set; }

        public ICommand SaveCommand { get; }

        public FeeManagementViewModel(IFeeService service)
        {
            _service = service;
            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            var model = new FeeStructureModel
            {
                AcademicYearId = SelectedAcademicYearId,
                ClassId = SelectedClassId,
//FeesDetails = FeesDetails
            };

            await _service.SaveFeeStructureAsync(model);
        }

    }
}
