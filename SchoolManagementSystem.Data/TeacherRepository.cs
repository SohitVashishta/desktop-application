using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagementSystem.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SchoolDbContext _context;

        // ✅ DbContext injected (NO new keyword)
        public TeacherRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherMaster>> GetAllAsync()
        {
            return await _context.Teachers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(TeacherMaster teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeacherMaster teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(x => x.TeacherId == id);

            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TeacherMaster> GetByIdAsync(int id)
        {
            return await _context.Teachers
                                 .FirstOrDefaultAsync(t => t.TeacherId == id);
        }

    }
}
