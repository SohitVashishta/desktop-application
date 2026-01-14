using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface ISystemRepository
    {
        Task<List<AuditLogDto>> GetAuditLogsAsync();
        Task<List<SystemSettingDto>> GetSettingsAsync();
        Task UpdateSettingAsync(string key, string value);
        Task RunBackupAsync();
    }
}
