using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class SystemService : ISystemService
    {
        private readonly ISystemRepository _repository = new SystemRepository();

        public Task<List<AuditLogDto>> GetAuditLogsAsync()
            => _repository.GetAuditLogsAsync();

        public Task<List<SystemSettingDto>> GetSettingsAsync()
            => _repository.GetSettingsAsync();

        public Task UpdateSettingAsync(string key, string value)
            => _repository.UpdateSettingAsync(key, value);

        public Task RunBackupAsync()
            => _repository.RunBackupAsync();
    }
}
