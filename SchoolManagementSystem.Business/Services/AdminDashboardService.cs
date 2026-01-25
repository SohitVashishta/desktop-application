using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repository;

        public AdminDashboardService(IAdminDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdminDashboardStats> GetDashboardAsync()
        {
            return await _repository.GetDashboardStatsAsync();
        }
    }
}
