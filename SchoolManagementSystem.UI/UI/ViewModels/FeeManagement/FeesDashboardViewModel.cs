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
    public class FeesDashboardViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _service;

        public FeesDashboardKpiModel Kpi { get; private set; }

        public ObservableCollection<MonthlyCollectionModel> MonthlyCollections { get; }
            = new();

        public ICommand LoadDashboardCommand { get; }

        public FeesDashboardViewModel(FeeService service)
        {
            _service = service;

            LoadDashboardCommand = new RelayCommand(async () =>
            {
                Kpi = await _service.GetKpiAsync();
                OnPropertyChanged(nameof(Kpi));

                MonthlyCollections.Clear();
                var data = await _service.GetMonthlyAsync();
                foreach (var item in data)
                    MonthlyCollections.Add(item);
            });
        }
    }

}
