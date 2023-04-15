using Microsoft.AspNetCore.Http;
using MiniExcelLibs;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class FileExtension
    {
        /// <summary>
        /// Save the file to a path.
        /// </summary>
        /// <param name="stream">The file upload.</param>
        /// <param name="filePath">The path to save the file.</param>
        /// <returns></returns>
        public static async Task<bool> SaveTo(this Stream stream, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using var fileStream = File.OpenWrite(filePath);
            var task = stream.CopyToAsync(fileStream);
            await task;
            while (!task.IsCompleted) Thread.Sleep(100);

            return task.IsCompleted;
        }

        /// <summary>
        /// From excel file data to list.
        /// </summary>
        /// <typeparam name="TClass">The list data class type.</typeparam>
        /// <param name="file">The data file.</param>
        /// <param name="map">The map from title to field.</param>
        /// <returns></returns>
        public static async Task<List<TClass>> ToList<TClass>(this IFormFile file, Dictionary<string, string> map = null!)
        {
            var propertities = typeof(TClass).GetProperties().ToDictionary(p => p.Name.ToUpper(), p => p);

            ISheet sheet;
            IWorkbook workbook;

            var stream = file.OpenReadStream();

            var extension = Path.GetExtension(file.FileName);
            if (extension.Equals(".xlsx"))
                workbook = new XSSFWorkbook(stream);
            else if (extension.Equals(".xls"))
                workbook = new HSSFWorkbook(stream);
            else
                throw new Exception("Not excel file!");

            var tmap = map?.ToDictionary(m => m.Key.ToUpper(), m => m.Value.ToUpper());
            string mapper(string s) => tmap != null && tmap.TryGetValue(s.ToUpper(), out var k) ? k : s.ToUpper();

            var list = new List<TClass>();
            if (workbook != null)
            {
                sheet = workbook.GetSheetAt(0);
                var titles = sheet.GetRow(0).Cells.ToDictionary(c => c.ColumnIndex, c => propertities.TryGetValue(mapper($"{c}"), out var v) ? v : null);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var item = Activator.CreateInstance<TClass>();
                    foreach (var t in titles.Where(x => x.Value != null))
                        t.Value!.SetValue(item, $"{sheet.GetRow(i).GetCell(t.Key)}");

                    list.Add(item);
                }
            }

            return await Task.FromResult(list);
        }

        /// <summary>
        /// From excel file data to list.
        /// </summary>
        /// <typeparam name="TClass">The list data class type.</typeparam>
        /// <param name="stream">The excel file stream.</param>
        /// <param name="map">The map from title to field.</param>
        /// <returns></returns>
        public static async Task<List<TClass>> ToList<TClass>(this Stream stream, Dictionary<string, string> map = null!)
        {
            var data = await MiniExcel.QueryAsync(stream);

            if (!data.Any())
                return new List<TClass>();

            var properties = typeof(TClass).GetProperties().Where(x => x.SetMethod != null).ToDictionary(x => x.Name.ToUpper(), x => x);

            return data
                .Select(x => ((object)x).GetType().GetProperties().ToDictionary(p => p.Name, p => Convert.ToString(p.GetValue(x))))
                .Select(x =>
                {
                    var instance = Activator.CreateInstance<TClass>();
                    foreach (var field in x)
                    {
                        var key = map.ContainsKey(field.Key) ? map[field.Key] : field.Key;
                        if (properties.ContainsKey(key.ToUpper()))
                        {
                            var property = properties[key.ToUpper()];
                            var propertyType = property.PropertyType;

                            if (propertyType == typeof(string))
                                property.SetValue(instance, field.Value);
                            else
                                property.SetValue(instance, Convert.ChangeType(field.Value, propertyType));
                        }
                    }
                    return instance;
                }).ToList();
        }
    }
}
