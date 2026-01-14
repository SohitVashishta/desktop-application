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
    public class InventoryManagementViewModel
    {
        private readonly IInventoryService _service = new InventoryService();

        public ObservableCollection<AssetDto> Assets { get; } = new();

        // Form fields
        public string AssetName { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } = "Active";
        public decimal PurchaseCost { get; set; }
        public string PurchaseDate { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand AddAssetCommand { get; }
        public ICommand UpdateStatusCommand { get; }

        public InventoryManagementViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddAssetCommand = new RelayCommand(async () => await AddAssetAsync());
            UpdateStatusCommand = new RelayCommand<AssetDto>(UpdateStatus);
        }

        private async Task LoadAsync()
        {
            Assets.Clear();
            foreach (var a in await _service.GetAssetsAsync())
                Assets.Add(a);
        }

        private async Task AddAssetAsync()
        {
            if (string.IsNullOrWhiteSpace(AssetName)) return;

            var asset = new AssetDto
            {
                AssetName = AssetName,
                Category = Category,
                Location = Location,
                Status = Status,
                PurchaseCost = PurchaseCost,
                PurchaseDate = PurchaseDate
            };

            await _service.AddAssetAsync(asset);
            Assets.Add(asset);

            AssetName = Category = Location = PurchaseDate = string.Empty;
            PurchaseCost = 0;
            Status = "Active";
        }

        private async void UpdateStatus(AssetDto asset)
        {
            if (asset == null) return;
            await _service.UpdateStatusAsync(asset.AssetId, asset.Status);
        }
    }
}
