using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZKSD.Utils
{
    public enum CellStyleEnum
    {
        头,
        url,
        时间,
        数字,
        钱,
        百分比,
        中文大写,
        科学计数法,
        默认
    }

    public static class NPOIHelper
    {
        public static IRow GetRowEx(this ISheet sheet, int rowNum)
        {
            IRow row = sheet.GetRow(rowNum);
            if (row == null)
                row = sheet.CreateRow(rowNum);
            return row;
        }
        public static ICell GetCellEx(this ISheet sheet, int row, int column)
        {
            IRow SheeetRow = sheet.GetRow(row);
            if (SheeetRow == null)
                SheeetRow = sheet.CreateRow(row);

            ICell cell = SheeetRow.GetCell(column);
            if (cell == null)
                cell = SheeetRow.CreateCell(column);

            return cell;
        }
        #region 定义单元格常用到样式
        static ICellStyle Getcellstyle(IWorkbook wb, CellStyleEnum str)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
            IFont font12 = wb.CreateFont();
            font12.FontHeightInPoints = 12;
            font12.FontName = "微软雅黑";


            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            //font.Underline = 1;下划线  


            IFont fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.OliveGreen.Blue.Index;
            fontcolorblue.IsItalic = true;//下划线  
            fontcolorblue.FontName = "微软雅黑";


            //边框  


            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Dotted;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Hair;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Dashed;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.OliveGreen.Blue.Index;
            cellStyle.TopBorderColor = HSSFColor.OliveGreen.Blue.Index;

            //背景图形，我没有用到过。感觉很丑  
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            // cellStyle.FillPattern = FillPatternType.NO_FILL;  
            cellStyle.FillBackgroundColor = HSSFColor.Blue.Index;

            //水平对齐  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //自动换行  
            cellStyle.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
            cellStyle.Indention = 0;

            //上面基本都是设共公的设置  
            //下面列出了常用的字段类型  
            switch (str)
            {
                case CellStyleEnum.头:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                    cellStyle.SetFont(font12);
                    break;
                case CellStyleEnum.时间:
                    IDataFormat datastyle = wb.CreateDataFormat();

                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.数字:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.钱:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.url:
                    fontcolorblue.Underline = FontUnderlineType.Single;
                    cellStyle.SetFont(fontcolorblue);
                    break;
                case CellStyleEnum.百分比:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.中文大写:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.科学计数法:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case CellStyleEnum.默认:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;


        }
        #endregion


        #region

        /// <summary>
        /// 添加图片
        /// </summary>
        /// dx1：起始单元格的x偏移量，如例子中的255表示直线起始位置距A1单元格左侧的距离；
        /// dy1：起始单元格的y偏移量，如例子中的125表示直线起始位置距A1单元格上侧的距离；
        /// dx2：终止单元格的x偏移量，如例子中的1023表示直线起始位置距C3单元格左侧的距离；
        /// dy2：终止单元格的y偏移量，如例子中的150表示直线起始位置距C3单元格上侧的距离；
        /// col1：起始单元格列序号，从0开始计算；
        /// row1：起始单元格行序号，从0开始计算，如例子中col1=0,row1=0就表示起始单元格为A1；
        /// col2：终止单元格列序号，从0开始计算；
        /// row2：终止单元格行序号，从0开始计算，如例子中col2=2,row2=2就表示起始单元格为C3；
        /// isAutoResize =ture 会根据图片的本身像素大小进行优化，默认设定一般情况下不使用；
        public static bool AddPicture(IWorkbook wb, ISheet sheet, byte[] bytes, PictureType pictureType,
            int dx1 = 0, int dy1 = 0, int dx2 = 0, int dy2 = 0, int col1 = 0, int row1 = 0, int col2 = 0, int row2 = 0, bool isAutoResize = false)
        {
            if (wb == null || sheet == null) return false;
            int pictureIdx = wb.AddPicture(bytes, pictureType);
            IDrawing patriarch = sheet.CreateDrawingPatriarch();
            if (wb is XSSFWorkbook)
            {
                XSSFClientAnchor anchor = new XSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
                IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                if (isAutoResize)
                {
                    pict.Resize();
                }
            }
            else
            {
                HSSFClientAnchor anchor = new HSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
                IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                if (isAutoResize)
                {
                    pict.Resize();
                }
            }
            return true;
        }
        #endregion

        public static IWorkbook GetWorkbook(string file)
        {

            IWorkbook workbook = null;
            try
            {
                using (FileStream fileStream = File.OpenRead(file))
                {
                    BinaryReader reader = new BinaryReader(fileStream);
                    string fileClass = string.Empty;
                    for (int i = 0; i < 2; i++)
                    {
                        fileClass += reader.ReadByte().ToString();
                    }

                    fileStream.Position = 0;

                    if (fileClass == "208207")
                    {
                        workbook = new HSSFWorkbook(fileStream);// 2003版本 .xls
                    }
                    else if (fileClass == "8075")
                    {
                        workbook = new XSSFWorkbook(fileStream);// 2007版本 .xlsx
                    }
                }
            }
            catch
            {

            }

            return workbook;
        }

        public static bool SaveWorkbook(IWorkbook wb, string file)
        {
            try
            {
                // 写入到文件中
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Write))
                {
                    wb.Write(fs);
                    fs.Close();
                    wb.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        //获取行高 单位：像素
        public static double GetSheetDefaultRowHeight_InPixel(ISheet sheet)
        {
            double cellHeight = sheet.DefaultRowHeightInPoints / 72 * 96;
            return cellHeight;
        }
        //设置行高 单位：像素
        //1‘=1440twip=25.4mm=72pt(磅point)=96px(像素pixel)    1px(像素pixel)=0.75pt(磅point)
        public static void SetSheetDefaultRowHeight_InPixel(ISheet sheet, double height)
        {
            sheet.DefaultRowHeight = (short)(height * 0.75 * 20);
        }

        public static void SetSheetDefaultColumnWidth_InPixel(ISheet sheet, double width)
        {
            double w_p = sheet.GetColumnWidthInPixels(0);
            double w_c = sheet.GetColumnWidth(0);
            double w = width * w_c / w_p;
            sheet.DefaultColumnWidth = (int)(w / 256);
        }


        //获取列宽 单位：像素
        public static double GetSheetColumnWidth_InPixel(ISheet sheet, int columnIndex)
        {
            double cellWidth = (double)sheet.GetColumnWidthInPixels(columnIndex);
            return cellWidth;
        }
        //设置列宽  单位：像素
        public static void SetSheetColumnWidth_InPixel(ISheet sheet, int columnIndex, double width)
        {
            double w_p = sheet.GetColumnWidthInPixels(columnIndex);
            double w_c = sheet.GetColumnWidth(columnIndex);
            double w = width * w_c / w_p;

            sheet.SetColumnWidth(columnIndex, (int)w);
        }


        public static void SetHyperlink(ISheet sheet1, int row1, int column1, ISheet sheet2, int row2, int column2 = 0)
        {
            #region 设置超链接
            row2++;
            string address = "#" + sheet2.SheetName + "!" + "A" + row2;//设置超链接点击跳转的地址 #sheet2!A 
            XSSFHyperlink link = new XSSFHyperlink(HyperlinkType.Document);
            link.Address = address;
            link.Tooltip = link.Address;
            sheet1.GetRow(row1).GetCell(column1).Hyperlink = link;
            #endregion
        }
        public static void Test1()
        {
            string file = "test.xlsx";


            IWorkbook workbook = new XSSFWorkbook();// 2007版本 .xlsx
            ISheet sheet1 = workbook.CreateSheet("sheet1");
            ISheet sheet2 = workbook.CreateSheet("sheet2");
            ISheet sheet3 = workbook.CreateSheet("sheet3");

            ///1‘=1440twip=25.4mm=72pt(磅point)=96px(像素pixel)    1px(像素pixel)=0.75pt(磅point)

            // sheet cell 高度 度量：point，单位 1/20 point；一个point的高度 == 96/72 个像素
            //sheet cell 宽度 度量：character 单位 1/256 character；
            sheet3.DefaultRowHeight = 100 * 20; // 153p

            sheet3.SetColumnWidth(0, 10 * 256);
            double w_p = sheet3.GetColumnWidthInPixels(0);
            double w_c = sheet3.GetColumnWidth(0);
            double h_point = sheet3.DefaultRowHeightInPoints;
            double h_pixel = h_point / 72.0 * 96.0;

            double w = h_pixel * w_c / w_p;
            sheet3.SetColumnWidth(0, (int)w);


            SetSheetDefaultRowHeight_InPixel(sheet3, 50);
            SetSheetDefaultColumnWidth_InPixel(sheet3, 50);
            SetSheetColumnWidth_InPixel(sheet3, 0, 100);

            sheet1.CreateRow(0);
            sheet1.GetRow(0).CreateCell(0);
            sheet1.GetRow(0).GetCell(0).SetCellValue("wxp");
            sheet1.GetRow(0).GetCell(0).CellStyle = NPOIHelper.Getcellstyle(workbook, CellStyleEnum.头);
            sheet1.CreateRow(1);
            sheet1.GetRow(1).CreateCell(1);
            sheet1.GetRow(1).GetCell(1).SetCellValue(123);

            sheet2.CreateRow(0);
            sheet2.GetRow(0).CreateCell(0);
            sheet2.GetRow(0).GetCell(0).SetCellValue("wxp");
            sheet2.GetRow(0).GetCell(0).CellStyle = NPOIHelper.Getcellstyle(workbook, CellStyleEnum.头);
            sheet2.CreateRow(1);
            sheet2.GetRow(1).CreateCell(1);
            sheet2.GetRow(1).GetCell(1).SetCellValue(123);


            #region 设置超链接
            XSSFHyperlink link = new XSSFHyperlink(HyperlinkType.Document);
            link.Label = "Label";

            link.Address = "#sheet2!A1";//设置超链接点击跳转的地址
            link.Tooltip = link.Address;
            sheet1.GetRow(1).GetCell(1).Hyperlink = link;//将链接方式赋值给单元格的Hyperlink即可将链接附加到单元格上 
            sheet1.GetRow(1).GetCell(1).CellStyle = NPOIHelper.Getcellstyle(workbook, CellStyleEnum.url);
            #endregion


            #region 添加图片



            //获取单元格像素尺寸
            double cellWidth = (double)sheet1.GetColumnWidthInPixels(0);

            ///1‘=1440twip=25.4mm=72pt(磅point)=96px(像素pixel)    
            ///1px(像素pixel)=0.75pt(磅point)
            double cellHeight = sheet1.DefaultRowHeightInPoints / 72 * 96;



            #endregion

            string FilePath = @"test.xlsx";
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
        }

        static CancellationTokenSource tokenSource = new CancellationTokenSource();

        //同时往多个sheet写入数据
        public static async void Test2()
        {
            IWorkbook workbook = new XSSFWorkbook();// 2007版本 .xlsx
            ISheet sheet1 = workbook.CreateSheet("sheet1");
            ISheet sheet2 = workbook.CreateSheet("sheet2");


            Task task1 = Task.Run(() =>
            {
                WriteData2Sheet(sheet1, 1);
            });
            Task task2 = Task.Run(() =>
            {
                WriteData2Sheet(sheet2, 2);
            });


            var rst = Task.WaitAll(new Task[] { task1, task2 }, new TimeSpan(0, 0, 5));

            if (rst == false)
            {
                tokenSource.Cancel();
            }

            string FilePath = @"test2.xlsx";
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
        }
        static void WriteData2Sheet(ISheet sheet, int value)
        {

            DateTime start = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {

                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(100);
                    row.CreateCell(j).SetCellValue(value);
                }
            }
            DateTime end = DateTime.Now;

            TimeSpan ts = end.Subtract(start);

            Console.Write(ts.TotalSeconds);
        }

    }
}
