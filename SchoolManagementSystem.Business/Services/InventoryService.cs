using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository
            = new InventoryRepository();

        public Task<List<AssetDto>> GetAssetsAsync()
            => _repository.GetAssetsAsync();

        public Task AddAssetAsync(AssetDto asset)
            => _repository.AddAssetAsync(asset);

        public Task UpdateStatusAsync(int assetId, string status)
            => _repository.UpdateStatusAsync(assetId, status);
    }

}
