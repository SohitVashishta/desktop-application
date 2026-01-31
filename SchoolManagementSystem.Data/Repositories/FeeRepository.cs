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
    public class FeeRepository : IFeeRepository
    {
        public Task<List<FeeDto>> GetFeesAsync()
        {
            return Task.FromResult(new List<FeeDto>
            {
                new FeeDto
                {
                    FeeId = 1,
                    StudentName = "Rahul Sharma",
                    Class = "10-A",
                    TotalAmount = 45000,
                    PaidAmount = 30000,
                    DueDate = "30-Sep-2026"
                },
                new FeeDto
                {
                    FeeId = 2,
                    StudentName = "Anita Verma",
                    Class = "9-B",
                    TotalAmount = 42000,
                    PaidAmount = 42000,
                    DueDate = "30-Sep-2026"
                }
            });
        }

        public async Task SaveFeeStructureAsync(FeeStructureModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeStructure_Save", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", model.ClassId);

            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Frequency", typeof(string));

            foreach (var f in model.Details)
                table.Rows.Add(f.FeeHeadId, f.Amount, f.Frequency);

            var p = cmd.Parameters.AddWithValue("@FeeDetails", table);
            p.SqlDbType = SqlDbType.Structured;

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public Task RecordPaymentAsync(int feeId, decimal amount)
        {
            // UPDATE PaidAmount
            return Task.CompletedTask;
        }
        public async Task<List<FeeHeadModel>> GetAllAsync()
        {
            var list = new List<FeeHeadModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeHead_GetAll", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            await ((SqlConnection)con).OpenAsync();

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new FeeHeadModel
                {
                    FeeHeadId = dr.GetInt32(dr.GetOrdinal("FeeHeadId")),
                    FeeHeadName = dr.GetString(dr.GetOrdinal("FeeHeadName")),

                    IsMandatory = dr.GetBoolean(dr.GetOrdinal("IsMandatory")),
                    IsActive = dr.GetBoolean(dr.GetOrdinal("IsActive")),

                    Amount = dr.GetDecimal(dr.GetOrdinal("Amount")),
                    Frequency = dr.GetString(dr.GetOrdinal("Frequency")),

                    ClassId = dr.GetInt32(dr.GetOrdinal("ClassId")),
                    ClassName = dr.GetString(dr.GetOrdinal("ClassName")),

                    DueDate = dr.GetDateTime(dr.GetOrdinal("DueDate")),
                    Description = dr.GetString(dr.GetOrdinal("Description"))
                });
            }

            return list;
        }


        public async Task SaveAsync(FeeHeadModel m)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeHead_Save", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FeeHeadId", (object)m.FeeHeadId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FeeHeadName", m.FeeHeadName);
            cmd.Parameters.AddWithValue("@IsMandatory", m.IsMandatory);
            cmd.Parameters.AddWithValue("@Amount", m.Amount);
            cmd.Parameters.AddWithValue("@Frequency", m.Frequency);
            cmd.Parameters.AddWithValue("@ClassId", m.ClassId);
            cmd.Parameters.AddWithValue("@DueDate", m.DueDate);
            cmd.Parameters.AddWithValue("@Description", m.Description);
            cmd.Parameters.AddWithValue("@IsActive", m.IsActive);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeHead_Delete", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FeeHeadId", id);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task SaveAsync(FeeStructureModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeStructure_Save", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", model.ClassId);

            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Frequency", typeof(string));

            foreach (var d in model.Details)
                table.Rows.Add(d.FeeHeadId, d.Amount, d.Frequency);

            var p = cmd.Parameters.AddWithValue("@Details", table);
            p.SqlDbType = SqlDbType.Structured;

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<FeeStructureDetailModel>> GetAsync(int yearId, int classId)
        {
            var list = new List<FeeStructureDetailModel>();
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeStructure_Get", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AcademicYearId", yearId);
            cmd.Parameters.AddWithValue("@ClassId", classId);

            await ((SqlConnection)con).OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new FeeStructureDetailModel
                {
                    FeeStructureDetailId = dr.GetInt32(0),
                    FeeHeadId = dr.GetInt32(1),
                    FeeHeadName = dr.GetString(2),
                    Amount = dr.GetDecimal(3),
                    Frequency = dr.GetString(4)
                });
            }
            return list;
        }
        public async Task SaveDiscountAsync(List<FeeDiscountModel> discounts)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeDiscount_Save", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            var table = new DataTable();
            table.Columns.Add("AcademicYearId", typeof(int));
            table.Columns.Add("ClassId", typeof(int));
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("DiscountAmount", typeof(decimal));
            table.Columns.Add("IsPercentage", typeof(bool));
            table.Columns.Add("Reason", typeof(string));

            foreach (var d in discounts)
                table.Rows.Add(d.AcademicYearId, d.ClassId, d.FeeHeadId,
                               d.DiscountAmount, d.IsPercentage, d.Reason);

            cmd.Parameters.AddWithValue("@Discounts", table);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public Task<List<FeeDiscountModel>> GetDiscountAsync(int academicYearId, int classId)
        {
            throw new NotImplementedException();
        }
        public async Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_StudentFeeAssignment_Save", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@TotalFees", model.TotalFees);
            cmd.Parameters.AddWithValue("@DiscountAmount", model.DiscountAmount);
            cmd.Parameters.AddWithValue("@NetFees", model.NetFees);

            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("FeeAmount", typeof(decimal));
            table.Columns.Add("DiscountAmount", typeof(decimal));
            table.Columns.Add("NetAmount", typeof(decimal));
            table.Columns.Add("DueDate", typeof(DateTime));

            foreach (var d in model.Details)
                table.Rows.Add(d.FeeHeadId, d.FeeAmount, d.DiscountAmount, d.NetAmount, d.DueDate);

            cmd.Parameters.AddWithValue("@Details", table);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<int> AddFeePaymentAsync(StudentFeePaymentModel m)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_StudentFeePayment_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", m.StudentId);
            cmd.Parameters.AddWithValue("@StudentFeeAssignmentId", m.StudentFeeAssignmentId);
            cmd.Parameters.AddWithValue("@ReceiptNo", m.ReceiptNo);
            cmd.Parameters.AddWithValue("@PaymentDate", m.PaymentDate);
            cmd.Parameters.AddWithValue("@PaymentMode", m.PaymentMode);
            cmd.Parameters.AddWithValue("@PaidAmount", m.PaidAmount);

            await ((SqlConnection)con).OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        public async Task ApplyLateFeesAsync()
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_StudentFee_ApplyLateFee", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task SaveReceiptAsync(FeeReceiptModel r)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(@"
        INSERT INTO StudentFeeReceipt
        (ReceiptNo, StudentId, PaidAmount, PaymentMode, PaymentDate)
        VALUES
        (@ReceiptNo, @StudentId, @PaidAmount, @PaymentMode, @PaymentDate)",
                (SqlConnection)con);

            cmd.Parameters.AddWithValue("@ReceiptNo", r.ReceiptNo);
            cmd.Parameters.AddWithValue("@StudentId", r.StudentId);
            cmd.Parameters.AddWithValue("@PaidAmount", r.PaidAmount);
            cmd.Parameters.AddWithValue("@PaymentMode", r.PaymentMode);
            cmd.Parameters.AddWithValue("@PaymentDate", r.PaymentDate);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeReport_DailyCollection", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromDate", from);
            cmd.Parameters.AddWithValue("@ToDate", to);

            await ((SqlConnection)con).OpenAsync();
            using var rdr = await cmd.ExecuteReaderAsync();

            var list = new List<DailyFeeCollectionModel>();
            while (await rdr.ReadAsync())
            {
                list.Add(new DailyFeeCollectionModel
                {
                    ReceiptNo = rdr["ReceiptNo"].ToString(),
                    StudentName = rdr["StudentName"].ToString(),
                    ClassName = rdr["ClassName"].ToString(),
                    PaidAmount = (decimal)rdr["PaidAmount"],
                    PaymentMode = rdr["PaymentMode"].ToString(),
                    PaymentDate = (DateTime)rdr["PaymentDate"]
                });
            }
            return list;
        }
        public async Task RefundAsync(FeeRefundModel model)
    {
        using var con = DbConnectionFactory.Create();
        using var cmd = new SqlCommand("usp_Fee_Refund", (SqlConnection)con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
        cmd.Parameters.AddWithValue("@ReceiptNo", model.ReceiptNo);
        cmd.Parameters.AddWithValue("@RefundAmount", model.RefundAmount);
        cmd.Parameters.AddWithValue("@RefundDate", model.RefundDate);
        cmd.Parameters.AddWithValue("@RefundMode", model.RefundMode);
        cmd.Parameters.AddWithValue("@Reason", model.Reason ?? "");

        await ((SqlConnection)con).OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
        public async Task ApplyFeeConcessionAsync(FeeConcessionModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Fee_ApplyConcession", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
            cmd.Parameters.AddWithValue("@ConcessionType", model.ConcessionType);
            cmd.Parameters.AddWithValue("@DiscountType", model.DiscountType);
            cmd.Parameters.AddWithValue("@DiscountValue", model.DiscountValue);
            cmd.Parameters.AddWithValue("@Reason", model.Reason ?? "");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId)
        {
            var list = new List<FeeReportModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Report_StudentFeeLedger", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await ((SqlConnection)con).OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();

            while (await rd.ReadAsync())
            {
                list.Add(new FeeReportModel
                {
                    StudentId = rd.GetInt32(0),
                    StudentName = rd.GetString(1),
                    ClassName = rd.GetString(2),
                    SectionName = rd.GetString(3),
                    TotalFees = rd.GetDecimal(4),
                    Concession = rd.GetDecimal(5),
                    PaidFees = rd.GetDecimal(6),
                    BalanceFees = rd.GetDecimal(7),
                    PaymentDate = rd.IsDBNull(8) ? null : rd.GetDateTime(8),
                    PaymentMode = rd.IsDBNull(9) ? null : rd.GetString(9)
                });
            }

            return list;
        }

        public async Task<FeesDashboardKpiModel> GetKpiAsync()
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeesDashboard_KPI", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();

            return await rd.ReadAsync()
                ? new FeesDashboardKpiModel
                {
                    TotalStudents = rd.GetInt32(0),
                    TotalFees = rd.GetDecimal(1),
                    CollectedFees = rd.GetDecimal(2),
                    PendingFees = rd.GetDecimal(3)
                }
                : new FeesDashboardKpiModel();
        }

        public async Task<List<MonthlyCollectionModel>> GetMonthlyAsync()
        {
            var list = new List<MonthlyCollectionModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeesDashboard_MonthlyCollection", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();

            while (await rd.ReadAsync())
            {
                list.Add(new MonthlyCollectionModel
                {
                    MonthName = rd.GetString(0),
                    CollectedAmount = rd.GetDecimal(1)
                });
            }

            return list;
        }

    }
}
