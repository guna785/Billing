using Namotion.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Billing.Helper
{
    public static class ExcelHelper
    {
        public static byte[] CreateExcelPackage(DataTable data)
        {
            byte[] reportBytes;
            using (var pakage = new ExcelPackage())
            {
                var workSheet = pakage.Workbook.Worksheets.Add(data.TableName);
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Cells["A1"].LoadFromDataTable(data, PrintHeaders: true);

                reportBytes= pakage.GetAsByteArray();
            }
            return reportBytes;

        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            // Use reflection to get the properties for the type we’re converting to a DataTable.
            var props = typeof(T).GetProperties();

            // Build the structure of the DataTable by converting the PropertyInfo[] into DataColumn[] using property list
            // Add each DataColumn to the DataTable at one time with the AddRange method.
            var dt = new DataTable();
            dt.Columns.AddRange(
              props.Where(x=>x.PropertyType==typeof(string) || x.PropertyType==typeof(DateTime)).Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());

            // Populate the property values to the DataTable
            source.ToList().ForEach(
              i => dt.Rows.Add(props.Where(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime)).Select(p => p.GetValue(i, null)).ToArray())
            );
            dt.TableName = typeof(T).Name;
            return dt;
        }

        
    }
}
