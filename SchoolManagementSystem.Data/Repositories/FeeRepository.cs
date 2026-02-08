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
        // ================= Fees =================

        public async Task<List<FeeDto>> GetFeesAsync()
        {
            var list = new List<FeeDto>();
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_Fee_GetList", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await ((SqlConnection)con).OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync())
                {
                    list.Add(new FeeDto
                    {
                        FeeId = rd.GetInt32(rd.GetOrdinal("FeeId")),
                        StudentName = rd["StudentName"].ToString(),
                        Class = rd["ClassName"].ToString(),
                        TotalAmount = (decimal)rd["TotalAmount"],
                        PaidAmount = (decimal)rd["PaidAmount"],
                        DueDate = Convert.ToDateTime(rd["DueDate"]).ToString("dd-MMM-yyyy")
                    });
                }
            }
            catch
            {
                throw;
            }

            return list;
        }

        public async Task RecordPaymentAsync(int feeId, decimal amount)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_Fee_RecordPayment", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FeeId", feeId);
                cmd.Parameters.AddWithValue("@Amount", amount);

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        // ================= Fee Head =================

        public async Task<List<FeeHeadModel>> GetFeeHeadsAsync(string feeType)
        {
            var list = new List<FeeHeadModel>();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeHead_GetAll", con)
            {
                CommandType = CommandType.StoredProcedure

            };
            cmd.Parameters.AddWithValue("@FeeType", feeType);
            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new FeeHeadModel
                {
                    FeeHeadId = dr.GetInt32(dr.GetOrdinal("FeeHeadId")),
                    FeeHeadName = dr.GetString(dr.GetOrdinal("FeeHeadName")),

                    IsMandatory = !dr.IsDBNull(dr.GetOrdinal("IsMandatory"))
                        && dr.GetBoolean(dr.GetOrdinal("IsMandatory")),

                    IsActive = !dr.IsDBNull(dr.GetOrdinal("IsActive"))
                        && dr.GetBoolean(dr.GetOrdinal("IsActive")),

                    IsRefundable = !dr.IsDBNull(dr.GetOrdinal("IsRefundable"))
                        && dr.GetBoolean(dr.GetOrdinal("IsRefundable"))
                });
            }

            return list;
        }

        public async Task<List<FeeHeadModel>> GetFeeHeadsAsync()
        {
            var list = new List<FeeHeadModel>();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeHead_GetAll", con)
            {
                CommandType = CommandType.StoredProcedure

            };
            cmd.Parameters.AddWithValue("@FeeType", "OneTime");
            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new FeeHeadModel
                {
                    FeeHeadId = dr.GetInt32(dr.GetOrdinal("FeeHeadId")),
                    FeeHeadName = dr.GetString(dr.GetOrdinal("FeeHeadName")),

                    IsMandatory = !dr.IsDBNull(dr.GetOrdinal("IsMandatory"))
                        && dr.GetBoolean(dr.GetOrdinal("IsMandatory")),

                    IsActive = !dr.IsDBNull(dr.GetOrdinal("IsActive"))
                        && dr.GetBoolean(dr.GetOrdinal("IsActive")),

                    IsRefundable = !dr.IsDBNull(dr.GetOrdinal("IsRefundable"))
                        && dr.GetBoolean(dr.GetOrdinal("IsRefundable"))
                });
            }

            return list;
        }
        public async Task SaveFeeHeadAsync(FeeHeadModel model)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeeHead_Save", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FeeHeadId", (object)model.FeeHeadId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FeeHeadName", model.FeeHeadName);
                cmd.Parameters.AddWithValue("@IsMandatory", model.IsMandatory);
                cmd.Parameters.AddWithValue("@Amount", model.Amount);
                cmd.Parameters.AddWithValue("@Frequency", model.Frequency);
                cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteFeeHeadAsync(int feeHeadId)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeeHead_Delete", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FeeHeadId", feeHeadId);

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        // ================= Fee Structure =================

        public async Task SaveFeeStructureAsync(FeeStructureModel model)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeStructure_Save", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@AcademicYearId", SqlDbType.Int).Value = model.AcademicYearId;
            cmd.Parameters.Add("@ClassId", SqlDbType.Int).Value = model.ClassId;
            cmd.Parameters.Add("@FeeType", SqlDbType.VarChar).Value = model.FeeType;

            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("Amount", typeof(decimal));

            foreach (var d in model.FeesDetails)
                table.Rows.Add(d.FeeHeadId, d.Amount);

            var p = cmd.Parameters.AddWithValue("@Details", table);
            p.SqlDbType = SqlDbType.Structured;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }



        public async Task<List<FeeStructureDetailModel>> GetFeeStructureAsync(
     int academicYearId,
     int classId,
     string feeType)
        {
            var list = new List<FeeStructureDetailModel>();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeStructure_Get", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AcademicYearId", academicYearId);
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@FeeType", feeType);

            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new FeeStructureDetailModel
                {
                    FeeStructureDetailId = dr.GetInt32(dr.GetOrdinal("FeeStructureDetailId")),
                    FeeHeadId = dr.GetInt32(dr.GetOrdinal("FeeHeadId")),
                    FeeHeadName = dr.GetString(dr.GetOrdinal("FeeHeadName")),
                    Amount = dr.GetDecimal(dr.GetOrdinal("Amount"))
                });
            }

            return list;
        }



        // ================= Discounts =================

        public async Task<List<FeeDiscountModel>> GetDiscountsAsync(int academicYearId, int classId)
        {
            var list = new List<FeeDiscountModel>();
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeeDiscount_Get", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@AcademicYearId", academicYearId);
                cmd.Parameters.AddWithValue("@ClassId", classId);

                await ((SqlConnection)con).OpenAsync();
                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    list.Add(new FeeDiscountModel
                    {
                        AcademicYearId = academicYearId,
                        ClassId = classId,
                        FeeHeadId = dr.GetInt32(0),
                        DiscountValue = dr.GetDecimal(1),
                        IsPercentage = dr.GetBoolean(2),
                        Reason = dr.GetString(3)
                    });
                }
            }
            catch
            {
                throw;
            }

            return list;
        }

        public async Task SaveDiscountsAsync(List<FeeDiscountModel> discounts)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeeDiscount_Save", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var table = new DataTable();
                table.Columns.Add("AcademicYearId", typeof(int));
                table.Columns.Add("ClassId", typeof(int));
                table.Columns.Add("FeeHeadId", typeof(int));
                table.Columns.Add("DiscountAmount", typeof(decimal));
                table.Columns.Add("IsPercentage", typeof(bool));
                table.Columns.Add("Reason", typeof(string));

                foreach (var d in discounts)
                    table.Rows.Add(d.AcademicYearId, d.ClassId, d.FeeHeadId,
                                   d.DiscountValue, d.IsPercentage, d.Reason);

                cmd.Parameters.AddWithValue("@Discounts", table);

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        // ================= Student Fee =================

        public async Task SaveFeeAssignmentAsync(StudentFeeAssignmentModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_StudentFeeAssignment_Save", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            // ================= HEADER =================
            cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = model.StudentId;
            cmd.Parameters.Add("@AcademicYearId", SqlDbType.Int).Value = model.AcademicYearId;
            cmd.Parameters.Add("@TotalFees", SqlDbType.Decimal).Value = model.TotalAmount;
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = model.TotalDiscount;
            cmd.Parameters.Add("@NetFees", SqlDbType.Decimal).Value = model.NetAmount;

            // ================= DETAILS (TVP) =================
            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("FeeAmount", typeof(decimal));
            table.Columns.Add("DiscountAmount", typeof(decimal));
            table.Columns.Add("NetAmount", typeof(decimal));
            table.Columns.Add("DueDate", typeof(DateTime));

            foreach (var d in model.Details)
            {
                table.Rows.Add(
                    d.FeeHeadId,
                    d.FeeAmount,          // ✅ mapped correctly
                    d.DiscountAmount,
                    d.NetAmount           // ✅ mapped correctly
                    //d.DueDate ?? (object)DBNull.Value
                );
            }

            var detailsParam = cmd.Parameters.AddWithValue("@Details", table);
            detailsParam.SqlDbType = SqlDbType.Structured;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<int> AddFeePaymentAsync(StudentFeePaymentModel model)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_StudentFeePayment_Add", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@StudentFeeAssignmentId", model.StudentFeeAssignmentId);
                cmd.Parameters.AddWithValue("@ReceiptNo", model.ReceiptNo);
                cmd.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                cmd.Parameters.AddWithValue("@PaidAmount", model.PaidAmount);

                await ((SqlConnection)con).OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task ApplyLateFeesAsync()
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_StudentFee_ApplyLateFee", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task RefundAsync(FeeRefundModel model)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_Fee_Refund", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@ReceiptNo", model.ReceiptNo);
                cmd.Parameters.AddWithValue("@RefundAmount", model.RefundAmount);
                cmd.Parameters.AddWithValue("@RefundDate", model.RefundDate);
                cmd.Parameters.AddWithValue("@RefundMode", model.RefundMode);
                cmd.Parameters.AddWithValue("@Reason", model.Reason ?? "");

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task ApplyFeeConcessionAsync(FeeConcessionModel model)
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_Fee_ApplyConcession", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@ConcessionType", model.ConcessionType);
                cmd.Parameters.AddWithValue("@DiscountType", model.DiscountType);
                cmd.Parameters.AddWithValue("@DiscountValue", model.DiscountValue);
                cmd.Parameters.AddWithValue("@Reason", model.Reason ?? "");

                await ((SqlConnection)con).OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        // ================= Receipt / Reports =================

        public async Task<string> SaveReceiptAsync(FeeReceiptModel model)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeReceipt_Save", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@FeeAssignmentId", model.FeeAssignmentId);
            cmd.Parameters.AddWithValue("@PaidAmount", model.PaidAmount);
            cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
            cmd.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);

            await con.OpenAsync();
            return Convert.ToString(await cmd.ExecuteScalarAsync());
        }


        public async Task<List<DailyFeeCollectionModel>> GetDailyCollectionAsync(DateTime from, DateTime to)
        {
            var list = new List<DailyFeeCollectionModel>();
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeeReport_DailyCollection", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FromDate", from);
                cmd.Parameters.AddWithValue("@ToDate", to);

                await ((SqlConnection)con).OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync())
                {
                    list.Add(new DailyFeeCollectionModel
                    {
                        ReceiptNo = rd["ReceiptNo"].ToString(),
                        StudentName = rd["StudentName"].ToString(),
                        ClassName = rd["ClassName"].ToString(),
                        PaidAmount = (decimal)rd["PaidAmount"],
                        PaymentMode = rd["PaymentMode"].ToString(),
                        PaymentDate = (DateTime)rd["PaymentDate"]
                    });
                }
            }
            catch
            {
                throw;
            }

            return list;
        }

        public async Task<List<MonthlyCollectionModel>> GetMonthlyAsync()
        {
            var list = new List<MonthlyCollectionModel>();
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeesDashboard_MonthlyCollection", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
            }
            catch
            {
                throw;
            }

            return list;
        }

        public async Task<List<FeeReportModel>> GetStudentLedgerAsync(int studentId)
        {
            var list = new List<FeeReportModel>();
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_Report_StudentFeeLedger", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
            }
            catch
            {
                throw;
            }

            return list;
        }

        public async Task<FeesDashboardKpiModel> GetKpiAsync()
        {
            try
            {
                using var con = DbConnectionFactory.Create();
                using var cmd = new SqlCommand("usp_FeesDashboard_KPI", (SqlConnection)con)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
            catch
            {
                throw;
            }
        }

        // ================= FEE RULES =================

        public async Task<List<FeeRuleRowModel>> GetFeeRulesAsync(
            int academicYearId,
            int classId,
            int? sectionId,
            string feeType)
        {
            var list = new List<FeeRuleRowModel>();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeRules_Get", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AcademicYearId", academicYearId);
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@SectionId", (object?)sectionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FeeType", feeType);

            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new FeeRuleRowModel
                {
                    FeeHeadId = dr.GetInt32(dr.GetOrdinal("FeeHeadId")),
                    FeeHeadName = dr.GetString(dr.GetOrdinal("FeeHeadName")),
                    DiscountType = dr.GetString(dr.GetOrdinal("DiscountType")),
                    DiscountValue = dr.GetDecimal(dr.GetOrdinal("DiscountValue")),
                    DueDate = dr.GetDateTime(dr.GetOrdinal("DueDate"))
                });
            }

            return list;
        }

        public async Task SaveFeeRulesAsync(
            int academicYearId,
            int classId,
            int? sectionId,
            string feeType,
            List<FeeRuleRowModel> rules)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeRules_Save", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AcademicYearId", academicYearId);
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@SectionId", (object?)sectionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FeeType", feeType);

            var table = new DataTable();
            table.Columns.Add("FeeHeadId", typeof(int));
            table.Columns.Add("DiscountType", typeof(string));
            table.Columns.Add("DiscountValue", typeof(decimal));
            table.Columns.Add("DueDate", typeof(DateTime));

            foreach (var r in rules)
                table.Rows.Add(r.FeeHeadId, r.DiscountType, r.DiscountValue, r.DueDate);

            var p = cmd.Parameters.AddWithValue("@Rules", table);
            p.SqlDbType = SqlDbType.Structured;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AssignFeeAsync(
    int studentId,
    int academicYearId,
    string feeType,
    decimal totalAmount,
    decimal totalDiscount,
    decimal netAmount,
    List<StudentFeeAssignmentDetailModel> details)
        {
            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_StudentFeeAssignment_Save", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            // ================= HEADER =================
            cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
            cmd.Parameters.Add("@AcademicYearId", SqlDbType.Int).Value = academicYearId;
            cmd.Parameters.Add("@FeeType", SqlDbType.VarChar, 50).Value = feeType;
            cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = totalAmount;
            cmd.Parameters.Add("@TotalDiscount", SqlDbType.Decimal).Value = totalDiscount;
            cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = netAmount;

            // ================= DETAILS (TVP) =================
            var dt = new DataTable();
            dt.Columns.Add("FeeHeadId", typeof(int));
            dt.Columns.Add("BaseAmount", typeof(decimal));
            dt.Columns.Add("DiscountAmount", typeof(decimal));
            dt.Columns.Add("NetAmount", typeof(decimal));
            dt.Columns.Add("DueDate", typeof(DateTime));

            foreach (var d in details)
            {
                dt.Rows.Add(
                    d.FeeHeadId,
                    d.FeeAmount,        // BaseAmount
                    d.DiscountAmount,
                    d.NetAmount,
                    d.DueDate ?? DateTime.Today
                );
            }

            var p = cmd.Parameters.AddWithValue("@Details", dt);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "dbo.StudentFeeAssignmentDetailType";

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<List<FeeReceiptModel>> GetReceiptsAsync(int studentId)
        {
            var list = new List<FeeReceiptModel>();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_FeeReceipt_GetByStudent", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new FeeReceiptModel
                {
                    ReceiptId = dr.GetInt32(dr.GetOrdinal("ReceiptId")),
                    ReceiptNo = dr.GetString(dr.GetOrdinal("ReceiptNo")),

                    StudentId = dr.GetInt32(dr.GetOrdinal("StudentId")),
                    StudentName = dr.GetString(dr.GetOrdinal("StudentName")),
                    ClassName = dr.GetString(dr.GetOrdinal("ClassName")),

                    NetFees = dr.GetDecimal(dr.GetOrdinal("NetFees")),
                    PaidAmount = dr.GetDecimal(dr.GetOrdinal("PaidAmount")),
                    BalanceAmount = dr.GetDecimal(dr.GetOrdinal("BalanceAmount")),

                    PaymentMode = dr.GetString(dr.GetOrdinal("PaymentMode")),
                    PaymentDate = dr.GetDateTime(dr.GetOrdinal("PaymentDate"))
                });
            }

            return list;
        }

    }
}
