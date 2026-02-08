using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public static class FeeCalculator
    {
        public static StudentFeeAssignmentModel Calculate(
            StudentModel student,
            List<FeeStructureModel> feeStructures,
            List<FeeRuleRowModel> feeRules   // unified Discount + DueDate
        )
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            var assignment = new StudentFeeAssignmentModel
            {
                StudentId = student.StudentId,
                AcademicYearId = student.AcademicYearId,
                Details = new List<StudentFeeAssignmentDetailModel>()
            };

            // ================= FLATTEN FEE STRUCTURE =================
            var feeHeads = feeStructures
                .Where(fs => fs.FeesDetails != null)
                .SelectMany(fs => fs.FeesDetails)
                .GroupBy(x => x.FeeHeadId)
                .Select(g => g.First());

            foreach (var fee in feeHeads)
            {
                var rule = feeRules?
                    .FirstOrDefault(r => r.FeeHeadId == fee.FeeHeadId);

                decimal discountAmount = 0;

                if (rule != null && rule.DiscountValue > 0)
                {
                    discountAmount = rule.DiscountType == "Percentage"
                        ? fee.Amount * rule.DiscountValue / 100
                        : rule.DiscountValue;
                }

                assignment.Details.Add(new StudentFeeAssignmentDetailModel
                {
                    FeeHeadId = fee.FeeHeadId,
                    FeeHeadName = fee.FeeHeadName,
                    FeeAmount = fee.Amount,
                    DiscountAmount = discountAmount,
                    //NetAmount = fee.Amount - discountAmount,
                    //DueDate = rule?.DueDate
                });
            }

            // ❌ DO NOT SET TOTALS HERE
            // Totals are auto-calculated in StudentFeeAssignmentModel

            return assignment;
        }
    }
}
