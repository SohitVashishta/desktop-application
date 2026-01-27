using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class StudentAdmissionRepository : IStudentAdmissionRepository
    {
        public async Task AddAsync(StudentProfileVM profile)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_Admission_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            MapCommonParameters(cmd, profile);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(StudentProfileVM profile)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_UpdateAdmission", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", profile.Student.StudentId);
            MapCommonParameters(cmd, profile);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static void MapCommonParameters(SqlCommand cmd, StudentProfileVM p)
        {
            // STUDENT
            cmd.Parameters.AddWithValue("@AdmissionNo", p.Student.AdmissionNo);
            cmd.Parameters.AddWithValue("@StudentName", p.Student.StudentName);
            cmd.Parameters.AddWithValue("@Gender", p.Student.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)p.Student.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodGroup", p.Student.BloodGroup ?? "");

            cmd.Parameters.AddWithValue("@ClassId", p.Student.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", p.Student.SectionId);
            cmd.Parameters.AddWithValue("@AcademicYearId", p.Student.AcademicYearId);
            cmd.Parameters.AddWithValue("@AdmissionDate", p.Student.AdmissionDate);

            cmd.Parameters.AddWithValue("@Category", p.Student.Category ?? "");
            cmd.Parameters.AddWithValue("@Religion", p.Student.Religion ?? "");
            cmd.Parameters.AddWithValue("@Nationality", p.Student.Nationality ?? "");

            // PARENT
            cmd.Parameters.AddWithValue("@FatherName", p.Parent.FatherName ?? "");
            cmd.Parameters.AddWithValue("@MotherName", p.Parent.MotherName ?? "");
            cmd.Parameters.AddWithValue("@GuardianName", p.Parent.GuardianName ?? "");
            cmd.Parameters.AddWithValue("@ContactNo", p.Parent.ContactNo ?? "");
            cmd.Parameters.AddWithValue("@Email", p.Parent.Email ?? "");

            // ADDRESS
            cmd.Parameters.AddWithValue("@AddressLine", p.Address.AddressLine ?? "");
            cmd.Parameters.AddWithValue("@City", p.Address.City ?? "");
            cmd.Parameters.AddWithValue("@State", p.Address.State ?? "");
            cmd.Parameters.AddWithValue("@Pincode", p.Address.Pincode ?? "");

            cmd.Parameters.AddWithValue("@CreatedBy", "SYSTEM");
            cmd.Parameters.AddWithValue("@UpdatedBy", "SYSTEM");
        }
        public async Task<int> SaveAdmissionAsync(StudentProfileVM profile)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_SaveAdmission", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // ===== STUDENT =====
            cmd.Parameters.AddWithValue("@StudentId", profile.Student.StudentId == 0 ? DBNull.Value : profile.Student.StudentId);
            cmd.Parameters.AddWithValue("@AdmissionNo", profile.Student.AdmissionNo);
            cmd.Parameters.AddWithValue("@FirstName", profile.Student.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", profile.Student.MiddleName ?? "");
            cmd.Parameters.AddWithValue("@LastName", profile.Student.LastName);
            cmd.Parameters.AddWithValue("@Gender", profile.Student.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", profile.Student.DateOfBirth);
            cmd.Parameters.AddWithValue("@AcademicYearId", profile.Student.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", profile.Student.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", profile.Student.SectionId);
            cmd.Parameters.AddWithValue("@AdmissionDate", profile.Student.AdmissionDate);

            // ===== ADDRESS =====
            cmd.Parameters.AddWithValue("@AddressLine", profile.Address.AddressLine);
            cmd.Parameters.AddWithValue("@City", profile.Address.City);
            cmd.Parameters.AddWithValue("@State", profile.Address.State);
            cmd.Parameters.AddWithValue("@Pincode", profile.Address.Pincode);

            // ===== PARENT =====
            cmd.Parameters.AddWithValue("@FatherName", profile.Parent.FatherName);
            cmd.Parameters.AddWithValue("@MotherName", profile.Parent.MotherName);
            cmd.Parameters.AddWithValue("@GuardianName", profile.Parent.GuardianName ?? "");
            cmd.Parameters.AddWithValue("@ContactNo", profile.Parent.ContactNo);
            cmd.Parameters.AddWithValue("@Email", profile.Parent.Email);

            // ===== FEES =====
            cmd.Parameters.AddWithValue("@TotalFees", profile.Fees.TotalFees);
            cmd.Parameters.AddWithValue("@PaidFees", profile.Fees.PaidFees);
            cmd.Parameters.AddWithValue("@PaymentMode", profile.Fees.PaymentMode);
            cmd.Parameters.AddWithValue("@ReceiptNo", profile.Fees.ReceiptNo);
            cmd.Parameters.AddWithValue("@PaymentDate", profile.Fees.PaymentDate);

            // ===== DOCUMENTS =====
            var table = new DataTable();
            table.Columns.Add("DocumentType");
            table.Columns.Add("FileName");
            table.Columns.Add("FilePath");

            foreach (var doc in profile.Documents)
                table.Rows.Add(doc.DocumentType, doc.FileName, doc.FilePath);

            cmd.Parameters.AddWithValue("@Documents", table)
               .SqlDbType = SqlDbType.Structured;

            await con.OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }


        public async Task<StudentProfileVM> GetByIdAsync(int studentId)
        {
            var profile = new StudentProfileVM
            {
                Student = new StudentModel(),
                Parent = new StudentParentModel(),
                Address = new StudentAddressModel(),
                ClassHistory = new List<StudentClassHistoryModel>()
            };

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_GetProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // STUDENT
            if (await reader.ReadAsync())
            {
                profile.Student.StudentId = studentId;
                profile.Student.StudentName = reader["StudentName"].ToString();
                profile.Student.Gender = reader["Gender"].ToString();
            }

            // PARENT
            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                profile.Parent.FatherName = reader["FatherName"].ToString();
                profile.Parent.ContactNo = reader["ContactNo"].ToString();
            }

            // ADDRESS
            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                profile.Address.AddressLine = reader["AddressLine"].ToString();
            }

            // HISTORY
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    profile.ClassHistory.Add(new StudentClassHistoryModel
                    {
                        AcademicYearId = (int)reader["AcademicYearId"],
                        ClassId = (int)reader["ClassId"],
                        SectionId = (int)reader["SectionId"]
                    });
                }
            }

            return profile;
        }
    }

}
