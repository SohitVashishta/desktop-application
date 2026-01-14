using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<RoleDto>> GetRolesAsync();
        Task<List<PermissionDto>> GetPermissionsAsync();
        Task<List<RolePermissionDto>> GetRolePermissionsAsync(int roleId);
        Task SaveRolePermissionAsync(RolePermissionDto model);
    }
}
