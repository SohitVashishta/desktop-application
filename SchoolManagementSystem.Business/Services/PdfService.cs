using SchoolManagementSystem.Models.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.Business.Services
{
    public class PdfService : IPdfService
    {
        public void GenerateReceipt(FeeReceiptModel model, string path)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Fee Receipt")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Text($"Receipt No: {model.ReceiptNo}");
                        col.Item().Text($"Student Name: {model.StudentName}");
                        col.Item().Text($"Class & Section: {model.ClassName}");
                        col.Item().Text($"Paid Amount: ₹ {model.PaidAmount}");
                        col.Item().Text($"Payment Mode: {model.PaymentMode}");
                        col.Item().Text($"Payment Date: {model.PaymentDate:dd-MMM-yyyy}");
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("This is a system generated receipt")
                        .FontSize(9)
                        .Italic();
                });
            })
            .GeneratePdf(path);
        }

        public void GenerateReport<T>(List<T> data, string title, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text(title)
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Total Records: {data.Count}");
                        col.Item().LineHorizontal(1);

                        foreach (var item in data)
                        {
                            col.Item().Text(item?.ToString());
                        }
                    });
                });
            })
            .GeneratePdf(filePath);
        }
    }
}
