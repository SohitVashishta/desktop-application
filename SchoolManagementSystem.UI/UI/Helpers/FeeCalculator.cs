using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public static class FeeCalculator
    {
        public static StudentFeeAssignmentModel Calculate(
            StudentModel student,
            List<FeeStructureModel> feeStructure,
            List<FeeDiscountModel> discounts,
            List<FeeDueDateModel> dueDates)
        {
            var assignment = new StudentFeeAssignmentModel
            {
                StudentId = student.StudentId,
                AcademicYearId = student.AcademicYearId
            };

            foreach (var fee in feeStructure)
            {
                var discount = discounts
                    .FirstOrDefault(x => x.FeeHeadId == fee.FeeHeadId);

                decimal discountAmount = 0;

                if (discount != null)
                {
                    discountAmount = discount.IsPercentage
                        ? fee.Amount * discount.DiscountAmount / 100
                        : discount.DiscountAmount;
                }

                var due = dueDates
                    .FirstOrDefault(x => x.FeeHeadId == fee.FeeHeadId);

                assignment.Details.Add(new StudentFeeAssignmentDetailModel
                {
                    FeeHeadId = fee.FeeHeadId,
                    FeeHeadName = fee.FeeHeadName,
                    FeeAmount = fee.Amount,
                    DiscountAmount = discountAmount,
                    NetAmount = fee.Amount - discountAmount,
                    DueDate = due?.DueDate ?? DateTime.Today
                });
            }

            assignment.TotalFees = assignment.Details.Sum(x => x.FeeAmount);
            assignment.DiscountAmount = assignment.Details.Sum(x => x.DiscountAmount);
            assignment.NetFees = assignment.Details.Sum(x => x.NetAmount);

            return assignment;
        }
    }

}
