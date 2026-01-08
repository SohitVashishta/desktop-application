using System.Collections.Generic;
using System.Linq;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class TeacherRepository
    {
        public List<Teacher> GetAll()
        {
            using var ctx = new SchoolDbContext();
            return ctx.Teachers.ToList();
        }

        public void Add(Teacher teacher)
        {
            using var ctx = new SchoolDbContext();
            ctx.Teachers.Add(teacher);
            ctx.SaveChanges();
        }

        public void Update(Teacher teacher)
        {
            using var ctx = new SchoolDbContext();
            ctx.Teachers.Update(teacher);
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            using var ctx = new SchoolDbContext();
            var teacher = ctx.Teachers.FirstOrDefault(x => x.TeacherId == id);
            if (teacher != null)
            {
                ctx.Teachers.Remove(teacher);
                ctx.SaveChanges();
            }
        }
    }
}
