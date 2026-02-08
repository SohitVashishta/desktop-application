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

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeRuleViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;

        public FeeRuleViewModel(IFeeService feeService)
        {
            _feeService = feeService;

            FeeTypes = new ObservableCollection<string>
        {
            "Monthly", "Quarterly", "Yearly", "OneTime"
        };

            DiscountTypes = new ObservableCollection<string>
        {
            "None", "Percentage", "Flat"
        };

            Rules = new ObservableCollection<FeeRuleRowModel>();

            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        // ================= CONTEXT =================
        public AcademicYearModel SelectedAcademicYear { get; set; }
        public ClassModel SelectedClass { get; set; }
        public SectionModel SelectedSection { get; set; }
        public string SelectedFeeType { get; set; }

        public ObservableCollection<string> FeeTypes { get; }
        public ObservableCollection<string> DiscountTypes { get; }

        // ================= GRID =================
        public ObservableCollection<FeeRuleRowModel> Rules { get; }

        public ICommand SaveCommand { get; }

        // ================= LOAD =================
        public async Task LoadAsync()
        {
            if (SelectedAcademicYear == null ||
                SelectedClass == null ||
                string.IsNullOrEmpty(SelectedFeeType))
                return;

            Rules.Clear();

            var heads = await _feeService.GetFeeHeadsAsync();
            var saved = await _feeService.GetFeeRulesAsync(
                SelectedAcademicYear.AcademicYearId,
                SelectedClass.ClassId,
                SelectedSection?.SectionId,
                SelectedFeeType);

            foreach (var h in heads)
            {
                var rule = saved.FirstOrDefault(x => x.FeeHeadId == h.FeeHeadId);

                Rules.Add(new FeeRuleRowModel
                {
                    FeeHeadId = h.FeeHeadId,
                    FeeHeadName = h.FeeHeadName,
                    DiscountType = rule?.DiscountType ?? "None",
                    DiscountValue = rule?.DiscountValue ?? 0,
                    DueDay = rule?.DueDay ?? 0,
                    GraceDays = rule?.GraceDays ?? 0
                });
            }
        }

        // ================= SAVE =================
        private async Task SaveAsync()
        {
            await _feeService.SaveFeeRulesAsync(
                SelectedAcademicYear.AcademicYearId,
                SelectedClass.ClassId,
                SelectedSection?.SectionId,
                SelectedFeeType,
                Rules.ToList());
        }
    }


}
