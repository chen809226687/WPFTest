using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKSD.Utils
{
    public class MyCsvHelper
    {
        public static bool WriteCsv<T>(List<T> datas, string filePath = "logs/TagData/tag.csv")
        {
            try
            {
                var currentPath = Directory.GetCurrentDirectory();
                var path = Path.Combine(currentPath, filePath);
                //判断文件夹是否存在
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                //判断文件是否存在
                if (!File.Exists(path))
                {

                    var file = File.Create(path);
                    file.Close();
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var writer = new StreamWriter(path, false, Encoding.GetEncoding("GB18030")))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(datas);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }
        public static List<T> ReadCsv<T>(string filePath)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var reader = new StreamReader(filePath, Encoding.GetEncoding("GB18030")))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    PrepareHeaderForMatch = args => args.Header.ToLower()
                }))
                {
                    var foos = csv.GetRecords<T>().ToList();

                    return foos.ToList();
                }
            }
            catch
            {

            }

            return new List<T>();
        }

    }
}
