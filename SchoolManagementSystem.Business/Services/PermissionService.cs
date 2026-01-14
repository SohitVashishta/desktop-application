using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class PermissionService
    {
        private readonly IPermissionRepository _repo;

        public PermissionService()
        {
            _repo = new PermissionRepository();
        }

        public Task<List<RoleDto>> GetRolesAsync()
            => _repo.GetRolesAsync();

        public Task<List<PermissionDto>> GetPermissionsAsync()
            => _repo.GetPermissionsAsync();

        public Task<List<RolePermissionDto>> GetRolePermissionsAsync(int roleId)
            => _repo.GetRolePermissionsAsync(roleId);

        public Task SaveAsync(RolePermissionDto model)
            => _repo.SaveRolePermissionAsync(model);
    }
}
