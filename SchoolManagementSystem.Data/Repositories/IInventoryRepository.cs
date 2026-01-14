using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{

    public interface IInventoryRepository
    {
        Task<List<AssetDto>> GetAssetsAsync();
        Task AddAssetAsync(AssetDto asset);
        Task UpdateStatusAsync(int assetId, string status);
    }
}
