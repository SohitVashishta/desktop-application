using SchoolManagementSystem.Models.Models;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IAdminDashboardRepository
    {
        Task<AdminDashboardStats> GetDashboardStatsAsync();
    }
}
