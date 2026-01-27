using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.Models.Models.SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        /* ================= GET ALL ================= */

        public async Task<List<StudentModel>> GetAllAsync()
        {
            var list = new List<StudentModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_GetAll", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new StudentModel
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    AdmissionNo = reader.GetString(reader.GetOrdinal("AdmissionNo")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    Gender = reader.GetString(reader.GetOrdinal("Gender")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                    SectionName = reader.GetString(reader.GetOrdinal("SectionName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }

        /* ================= ADD ================= */

        public async Task<int> AddAsync(StudentModel s)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AdmissionNo", s.AdmissionNo);
            cmd.Parameters.AddWithValue("@StudentName", s.StudentName);
            cmd.Parameters.AddWithValue("@Gender", s.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)s.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClassId", s.ClassName);
            cmd.Parameters.AddWithValue("@SectionId", s.SectionName);
            cmd.Parameters.AddWithValue("@AcademicYearId", s.AcademicYearId);
            cmd.Parameters.AddWithValue("@CreatedBy", "System");

            await ((SqlConnection)con).OpenAsync();

            // 🔥 usp_Student_Add MUST RETURN SCOPE_IDENTITY()
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        /* ================= UPDATE ================= */

        public async Task UpdateAsync(StudentModel s)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_Update", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", s.StudentId);
            cmd.Parameters.AddWithValue("@StudentName", s.StudentName);
            cmd.Parameters.AddWithValue("@Gender", s.Gender);
            cmd.Parameters.AddWithValue("@ClassId", s.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", s.SectionId);
            cmd.Parameters.AddWithValue("@UpdatedBy", "System");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= STATUS ================= */

        public async Task ToggleStatusAsync(int studentId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_ToggleStatus", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= CLASS HISTORY ================= */

        public async Task InsertClassHistoryAsync(StudentClassHistoryModel h)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_ClassHistory_Insert",
                                           (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", h.StudentId);
            cmd.Parameters.AddWithValue("@AcademicYearId", h.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", h.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", h.SectionId);
            cmd.Parameters.AddWithValue("@IsCurrent", h.IsCurrent);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MarkHistoryNotCurrentAsync(int studentId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(
                @"UPDATE StudentClassHistory
                  SET IsCurrent = 0
                  WHERE StudentId = @StudentId",
                (SqlConnection)con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= PROMOTION ================= */

        public async Task UpdateClassSectionAsync(int studentId, int classId, int sectionId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(
                @"UPDATE StudentMaster
                  SET ClassId = @ClassId,
                      SectionId = @SectionId,
                      UpdatedOn = GETDATE()
                  WHERE StudentId = @StudentId",
                (SqlConnection)con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@SectionId", sectionId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<int> AddAdmissionAsync(StudentProfileVM profile)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_SaveAdmission", con);
            cmd.CommandType = CommandType.StoredProcedure;

            var s = profile.Student;
            var a = profile.Address;
            var p = profile.Parent;
            var f = profile.Fees;

            /* ================= STUDENT ================= */

            cmd.Parameters.AddWithValue("@StudentId",
        profile.Student.StudentId == 0 ? DBNull.Value : profile.Student.StudentId);
            cmd.Parameters.AddWithValue("@AdmissionNo", s.AdmissionNo);
            cmd.Parameters.AddWithValue("@FirstName", s.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", (object?)s.MiddleName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastName", s.LastName);
            cmd.Parameters.AddWithValue("@Gender", s.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", s.DateOfBirth);
            cmd.Parameters.AddWithValue("@AcademicYearId", s.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", s.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", s.SectionId);
            cmd.Parameters.AddWithValue("@AdmissionDate", s.AdmissionDate);

            /* ================= ADDRESS ================= */

            cmd.Parameters.AddWithValue("@AddressLine", a.AddressLine);
            cmd.Parameters.AddWithValue("@City", a.City);
            cmd.Parameters.AddWithValue("@State", a.State);
            cmd.Parameters.AddWithValue("@Pincode", a.Pincode);

            /* ================= PARENT ================= */

            cmd.Parameters.AddWithValue("@FatherName", p.FatherName);
            cmd.Parameters.AddWithValue("@MotherName", (object?)p.MotherName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GuardianName", (object?)p.GuardianName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactNo", p.FatherMobile);
            cmd.Parameters.AddWithValue("@Email", (object?)p.Email ?? DBNull.Value);

            /* ================= FEES ================= */

            cmd.Parameters.AddWithValue("@TotalFees", f.TotalFees);
            cmd.Parameters.AddWithValue("@PaidFees", f.PaidFees);
            cmd.Parameters.AddWithValue("@PaymentMode", f.PaymentMode);
            cmd.Parameters.AddWithValue("@ReceiptNo", Generate());
            cmd.Parameters.AddWithValue("@PaymentDate", f.PaymentDate);

            /* ================= DOCUMENTS (TVP) ================= */

            var docTable = new DataTable();
            docTable.Columns.Add("DocumentType", typeof(string));
            docTable.Columns.Add("FileName", typeof(string));
            docTable.Columns.Add("FilePath", typeof(string));

            foreach (var d in profile.Documents)
            {
                docTable.Rows.Add(d.DocumentType, d.FileName, d.FilePath);
            }

            var docParam = cmd.Parameters.AddWithValue("@Documents", docTable);
            docParam.SqlDbType = SqlDbType.Structured;
            docParam.TypeName = "StudentDocumentType";

            /* ================= EXECUTE ================= */

            await con.OpenAsync();
            var studentId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            return studentId;
        }
        public async Task UpdateAdmissionAsync(StudentAdmissionModel model)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            await con.OpenAsync();

            using var cmd = new SqlCommand("usp_Student_UpdateAdmission", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
            cmd.Parameters.AddWithValue("@StudentName", model.StudentName);
            cmd.Parameters.AddWithValue("@Gender", model.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodGroup", model.BloodGroup);

            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", model.SectionId);
            cmd.Parameters.AddWithValue("@AdmissionDate", model.AdmissionDate);

            cmd.Parameters.AddWithValue("@FatherName", model.FatherName);
            cmd.Parameters.AddWithValue("@MotherName", model.MotherName);
            cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Address", model.Address);

            cmd.Parameters.AddWithValue("@Category", model.Category);
            cmd.Parameters.AddWithValue("@Religion", model.Religion);
            cmd.Parameters.AddWithValue("@Nationality", model.Nationality);

            await cmd.ExecuteNonQueryAsync();
        }

       
            private static readonly object _lock = new();
            private static int _lastNumber = 0;
            private static int _currentYear = DateTime.Now.Year;

            public static string Generate()
            {
                lock (_lock)
                {
                    int year = DateTime.Now.Year;

                    // Reset sequence if year changed
                    if (year != _currentYear)
                    {
                        _currentYear = year;
                        _lastNumber = 0;
                    }

                    _lastNumber++;

                    return $"RCPT-{year}-{_lastNumber:D6}";
                }
            }
        


    }
}
