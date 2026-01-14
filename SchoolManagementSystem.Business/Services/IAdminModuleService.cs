using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IAdminModuleService
    {
        Task<List<AdminModuleItem>> GetAdminModulesAsync();
    }
}
