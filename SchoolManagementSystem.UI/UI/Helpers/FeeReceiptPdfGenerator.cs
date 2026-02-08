using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using SchoolManagementSystem.Models.Models;
using System.IO;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class FeeReceiptPdfGenerator
    {
        public static void Generate(FeeReceiptModel r, string filePath)
        {
            // Ensure directory exists
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            using var doc = new Document(pdf);

            // Fonts
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            /* ================= HEADER ================= */

            doc.Add(new Paragraph("SCHOOL NAME")
                .SetFont(boldFont)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16));

            doc.Add(new Paragraph("Fee Receipt")
                .SetFont(normalFont)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetMarginBottom(15));

            /* ================= STUDENT INFO ================= */

            doc.Add(new Paragraph($"Receipt No : {r.ReceiptNo}").SetFont(normalFont));
            doc.Add(new Paragraph($"Date       : {r.PaymentDate:dd-MMM-yyyy}").SetFont(normalFont));
            doc.Add(new Paragraph($"Student    : {r.StudentName}").SetFont(normalFont));
            doc.Add(new Paragraph($"Class      : {r.ClassName}")
                .SetFont(normalFont)
                .SetMarginBottom(15));

            /* ================= FEE TABLE ================= */

            Table table = new Table(2)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetBorder(new SolidBorder(1));

            AddRow(table, "Net Fees", r.NetFees, boldFont, normalFont);
            AddRow(table, "Late Fee", r.LateFee, boldFont, normalFont);
            AddRow(table, "Paid Amount", r.PaidAmount, boldFont, normalFont);
            AddRow(table, "Balance Amount", r.BalanceAmount, boldFont, normalFont);

            doc.Add(table);

            /* ================= FOOTER ================= */

            doc.Add(new Paragraph($"\nPayment Mode : {r.PaymentMode}")
                .SetFont(normalFont));

            doc.Add(new Paragraph("\nAuthorized Signature")
                .SetFont(boldFont)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginTop(30));

            doc.Close();
        }

        /* ================= TABLE ROW HELPER ================= */

        private static void AddRow(
            Table table,
            string label,
            decimal value,
            PdfFont boldFont,
            PdfFont normalFont)
        {
            table.AddCell(new Cell()
                .Add(new Paragraph(label).SetFont(boldFont))
                .SetBorder(new SolidBorder(1)));

            table.AddCell(new Cell()
                .Add(new Paragraph(value.ToString("0.00")).SetFont(normalFont))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(new SolidBorder(1)));
        }
    }
}
