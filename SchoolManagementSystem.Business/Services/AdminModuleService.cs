using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class AdminModuleService : IAdminModuleService
    {
        public Task<List<AdminModuleItem>> GetAdminModulesAsync()
        {
            // 🔒 Admin full control modules
            return Task.FromResult(new List<AdminModuleItem>
            {
                new() { Title="User & Role Management", Icon="", ModuleKey="USER_ROLE",
                    Description="Create users, assign roles, reset passwords" },

                new() { Title="Student Management", Icon="", ModuleKey="STUDENTS",
                    Description="Profiles, enrollment, class & section assignment" },

                new() { Title="Attendance Management", Icon="", ModuleKey="ATTENDANCE",
                    Description="Student, teacher & staff attendance reports" },

                new() { Title="Examination & Grading", Icon="", ModuleKey="EXAMS",
                    Description="Exams, grading rules, report cards" },

                new() { Title="Fee Management", Icon="", ModuleKey="FEES",
                    Description="Fee structure, payments, receipts" },

                new() { Title="Timetable Management", Icon="", ModuleKey="TIMETABLE",
                    Description="Class & teacher scheduling" },

                new() { Title="Communication", Icon="", ModuleKey="COMMUNICATION",
                    Description="Announcements, alerts, messaging" },

                new() { Title="Library Management", Icon="", ModuleKey="LIBRARY",
                    Description="Books, issue/return, fines" },

                new() { Title="HR & Payroll", Icon="", ModuleKey="HR",
                    Description="Staff profiles, payroll, deductions" },

                new() { Title="Inventory & Assets", Icon="", ModuleKey="INVENTORY",
                    Description="Assets, maintenance & purchase logs" },

                new() { Title="System & Security", Icon="", ModuleKey="SYSTEM",
                    Description="Audit logs, backups, system settings" }
            });
        }
    }
}
