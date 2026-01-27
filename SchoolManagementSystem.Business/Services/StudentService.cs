using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.Models.Models.SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /* ================= GET ================= */

        public async Task<List<StudentModel>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        /* ================= ADD ================= */

        public async Task<int> AddAsync(StudentModel student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            int studentId = await _studentRepository.AddAsync(student);

            // 🔥 INSERT CLASS HISTORY
            await _studentRepository.InsertClassHistoryAsync(
                new StudentClassHistoryModel
                {
                    StudentId = studentId,
                    AcademicYearId = student.AcademicYearId,
                    ClassId = student.ClassId,
                    SectionId = student.SectionId,
                    IsCurrent = true
                });

            return studentId;
        }

        /* ================= UPDATE ================= */

        public async Task UpdateAsync(StudentModel student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            await _studentRepository.UpdateAsync(student);
        }

        /* ================= STATUS ================= */

        public async Task ToggleStatusAsync(int studentId)
        {
            await _studentRepository.ToggleStatusAsync(studentId);
        }

        /* ================= BULK UPLOAD ================= */

        public async Task BulkUploadAsync(List<StudentModel> students)
        {
            if (students == null || students.Count == 0)
                throw new Exception("No students found in file");

            foreach (var s in students)
            {
                // BASIC VALIDATION
                //if (string.IsNullOrWhiteSpace(s.AdmissionNo) ||
                //    string.IsNullOrWhiteSpace(s.StudentName) ||
                //    s.ClassId <= 0 ||
                //    s.SectionId <= 0 ||
                //    s.AcademicYearId <= 0)
                //{
                //    throw new Exception(
                //        $"Invalid data for AdmissionNo: {s.AdmissionNo}");
                //}

                int studentId = await _studentRepository.AddAsync(s);

                await _studentRepository.InsertClassHistoryAsync(
                    new StudentClassHistoryModel
                    {
                        StudentId = studentId,
                        AcademicYearId = s.AcademicYearId,
                        ClassId = s.ClassId,
                        SectionId = s.SectionId,
                        IsCurrent = true
                    });
            }
        }

        /* ================= PROMOTION ================= */

        public async Task PromoteAsync(
            int studentId,
            int nextClassId,
            int nextSectionId,
            int academicYearId)
        {
            // 🔥 MARK OLD HISTORY NOT CURRENT
            await _studentRepository.MarkHistoryNotCurrentAsync(studentId);

            // 🔥 INSERT NEW HISTORY
            await _studentRepository.InsertClassHistoryAsync(
                new StudentClassHistoryModel
                {
                    StudentId = studentId,
                    AcademicYearId = academicYearId,
                    ClassId = nextClassId,
                    SectionId = nextSectionId,
                    IsCurrent = true
                });

            // 🔥 UPDATE MASTER
            await _studentRepository.UpdateClassSectionAsync(
                studentId,
                nextClassId,
                nextSectionId);
        }

        public Task<StudentProfileVM> GetProfileAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        // ================= ADD =================
        public async Task AddAsync(StudentProfileVM profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            await _studentRepository.AddAsync(profile.Student);
        }

        // ================= UPDATE =================
        public async Task UpdateAsync(StudentProfileVM profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            await _studentRepository.UpdateAsync(profile.Student);
        }
        public async Task SaveAdmissionAsync(StudentProfileVM model)
        {
            await _studentRepository.AddAdmissionAsync(model);
        }
        public async Task UpdateAdmissionAsync(StudentAdmissionModel model)
        {
            await _studentRepository.UpdateAdmissionAsync(model);
        }

    }
}
