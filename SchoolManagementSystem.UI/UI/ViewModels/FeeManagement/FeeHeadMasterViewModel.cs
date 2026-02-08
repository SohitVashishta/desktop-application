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
    public class FeeHeadMasterViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;

        public FeeHeadMasterViewModel()
        {
            _feeService = new FeeService();

            FeeHeads = new ObservableCollection<FeeHeadModel>();

            AddNewCommand = new RelayCommand(AddNew);
            EditCommand = new RelayCommand<FeeHeadModel>(Edit);
            DeleteCommand = new RelayCommand<FeeHeadModel>(Delete);

            LoadData();
        }

        // ================= COLLECTION =================
        public ObservableCollection<FeeHeadModel> FeeHeads { get; set; }

        // ================= COMMANDS =================
        public ICommand AddNewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        // ================= METHODS =================
        private async void LoadData()
        {
            FeeHeads.Clear();
            var data = await _feeService.GetFeeHeadsAsync("OneTime");

            foreach (var item in data)
                FeeHeads.Add(item);
        }

        private async void AddNew()
        {
            var model = new FeeHeadModel
            {
                FeeHeadName = "New Fee Head",
                IsMandatory = false,
                IsRefundable = false,
                IsActive = true
            };

            await _feeService.SaveFeeHeadAsync(model);
            LoadData();
        }

        private async void Edit(FeeHeadModel model)
        {
            if (model == null) return;

            await _feeService.SaveFeeHeadAsync(model);
            LoadData();
        }

        private async void Delete(FeeHeadModel model)
        {
            if (model == null) return;

            await _feeService.DeleteFeeHeadAsync(model.FeeHeadId);
            LoadData();
        }
    }
}
