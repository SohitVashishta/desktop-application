using Dapper;
using SchoolManagementSystem.Models.Models;
using System.Data;

namespace SchoolManagementSystem.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbConnection _db;

        public PermissionRepository()
        {
            _db = DbConnectionFactory.Create();
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            return (await _db.QueryAsync<RoleDto>(
                "SELECT Id AS RoleId, Name FROM Roles")).ToList();
        }

        public async Task<List<PermissionDto>> GetPermissionsAsync()
        {
            return (await _db.QueryAsync<PermissionDto>(
                "SELECT Id AS PermissionId, Code, Description FROM Permissions")).ToList();
        }

        public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(int roleId)
        {
            return (await _db.QueryAsync<RolePermissionDto>(
                "SELECT RoleId, PermissionId, IsAllowed FROM RolePermissions WHERE RoleId=@roleId",
                new { roleId })).ToList();
        }

        public async Task SaveRolePermissionAsync(RolePermissionDto model)
        {
            var sql = @"
                MERGE RolePermissions AS target
                USING (SELECT @RoleId AS RoleId, @PermissionId AS PermissionId) AS source
                ON target.RoleId = source.RoleId AND target.PermissionId = source.PermissionId
                WHEN MATCHED THEN
                    UPDATE SET IsAllowed = @IsAllowed
                WHEN NOT MATCHED THEN
                    INSERT (RoleId, PermissionId, IsAllowed)
                    VALUES (@RoleId, @PermissionId, @IsAllowed);";

            await _db.ExecuteAsync(sql, model);
        }
    }
}
