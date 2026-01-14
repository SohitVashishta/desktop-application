using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly List<AuditLogDto> _logs = new()
        {
            new AuditLogDto
            {
                Id = 1,
                User = "Admin",
                Action = "Created Student",
                Module = "Student Management",
                DateTime = "01-Feb-2026 10:30"
            },
            new AuditLogDto
            {
                Id = 2,
                User = "Admin",
                Action = "Updated Fee Structure",
                Module = "Fee Management",
                DateTime = "02-Feb-2026 14:15"
            }
        };

        private readonly List<SystemSettingDto> _settings = new()
        {
            new SystemSettingDto { Key = "AutoBackup", Value = "Enabled" },
            new SystemSettingDto { Key = "PasswordPolicy", Value = "Strong" }
        };

        public Task<List<AuditLogDto>> GetAuditLogsAsync()
            => Task.FromResult(_logs);

        public Task<List<SystemSettingDto>> GetSettingsAsync()
            => Task.FromResult(_settings);

        public Task UpdateSettingAsync(string key, string value)
        {
            var s = _settings.Find(x => x.Key == key);
            if (s != null) s.Value = value;
            return Task.CompletedTask;
        }

        public Task RunBackupAsync()
        {
            // Trigger DB/File backup
            return Task.CompletedTask;
        }
    }
}
