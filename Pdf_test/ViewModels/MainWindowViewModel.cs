using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace Pdf_test.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public void test()
        {
            FileStream fs;
            // 创建一个新的PDF文档
            using (fs = new FileStream("C:/Users/80922/Desktop/日志/03.pdf", FileMode.Create))
            {
             
                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                doc.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                BaseFont baseFont1 = BaseFont.CreateFont("C:\\WINDOWS\\FONTS\\SIMKAI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                // 创建一个Font对象，设置字体为中文字体
                Font chineseFontStyle = new Font(baseFont1, 14);

                // 添加内容到PDF文档

                Paragraph paragraph1 = new Paragraph("日志报表", chineseFontStyle); paragraph1.Alignment = Element.ALIGN_CENTER; doc.Add(paragraph1);

                doc.Add(new Chunk(new LineSeparator()));

                Paragraph paragraph2 = new Paragraph("导出信息", chineseFontStyle); paragraph2.Alignment = Element.ALIGN_LEFT; doc.Add(paragraph2);


                PdfPTable table = new PdfPTable(3);//为pdfpTable的构造函数传入整数3，pdfpTable被初始化为一个三列的表格

                PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));

                cell.Colspan = 3;

                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                table.AddCell(cell);

                table.AddCell("Col 1 Row 1");
                table.AddCell("Col 2 Row 1");
                table.AddCell("Col 3 Row 1");

                table.AddCell("Col 1 Row 2");
                table.AddCell("Col 2 Row 2");
                table.AddCell("Col 3 Row 2");

                doc.Add(table);

                doc.Close();
                writer.Close();

             


            }

        }
    }
}
