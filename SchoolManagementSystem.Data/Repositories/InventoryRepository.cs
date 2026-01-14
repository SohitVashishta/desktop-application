using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly List<AssetDto> _assets = new()
        {
            new AssetDto
            {
                AssetId = 1,
                AssetName = "Physics Lab Microscope",
                Category = "Lab Equipment",
                Location = "Physics Lab",
                Status = "Active",
                PurchaseCost = 25000,
                PurchaseDate = "12-Jan-2025"
            },
            new AssetDto
            {
                AssetId = 2,
                AssetName = "Classroom Bench",
                Category = "Furniture",
                Location = "Class 10-A",
                Status = "Maintenance",
                PurchaseCost = 8000,
                PurchaseDate = "20-Mar-2024"
            }
        };

        public Task<List<AssetDto>> GetAssetsAsync()
            => Task.FromResult(_assets);

        public Task AddAssetAsync(AssetDto asset)
        {
            asset.AssetId = _assets.Count + 1;
            _assets.Add(asset);
            return Task.CompletedTask;
        }

        public Task UpdateStatusAsync(int assetId, string status)
        {
            var asset = _assets.FirstOrDefault(a => a.AssetId == assetId);
            if (asset != null)
                asset.Status = status;

            return Task.CompletedTask;
        }
    }
}
