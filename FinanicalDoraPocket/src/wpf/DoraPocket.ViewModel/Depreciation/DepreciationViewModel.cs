using DoraPocket.Common;
using DoraPocket.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace DoraPocket.ViewModel.Depreciation
{
    public class DepreciationViewModel : BaseViewModel
    {
        #region fields
        private string filePath;
        private string sheetName = "资产清单";
        private string originalValueColumnName = "原币原值";
        private string residualValueRateColumnName = "净残值率%";
        private string monthOfUseColumnName = "使用月限";
        private string addMonthColumnName = "增加月份";
        private int baseYear = DateTime.Now.Year;

        private readonly IEventSource eventSource;
        #endregion

        #region construct
        public DepreciationViewModel()
        {
            eventSource = ServiceProviderAccessor.Current.GetRequiredService<IEventSource>();
        }
        #endregion

        #region properties
        /// <summary>
        /// 需要进行折旧计算的excel文件路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { SetProperty(ref filePath, value); }
        }

        /// <summary>
        /// 需要进行折旧计算的sheet名
        /// 若为空，则默认全部sheet都进行计算；若非空，多个待计算的sheet用英文','分隔；形如：资产清单
        /// </summary>
        public string SheetName
        {
            get { return sheetName; }
            set { SetProperty(ref sheetName, value); }
        }

        /// <summary>
        /// 列名：原币原值，其对应的cell里的值形如5467.92
        /// </summary>
        public string OriginalValueColumnName
        {
            get { return originalValueColumnName; }
            set { SetProperty(ref originalValueColumnName, value); }
        }

        /// <summary>
        /// 列名：净残值率%，其对应的cell里的值形如5
        /// </summary>
        public string ResidualValueRateColumnName
        {
            get { return residualValueRateColumnName; }
            set { SetProperty(ref residualValueRateColumnName, value); }
        }

        /// <summary>
        /// 列名：使用月限，其对应的cell里的值形如12
        /// </summary>
        public string MonthOfUseColumnName
        {
            get { return monthOfUseColumnName; }
            set { SetProperty(ref monthOfUseColumnName, value); }
        }

        /// <summary>
        /// 列名：增加月份，其对应的cell里的值形如202008
        /// </summary>
        public string AddMonthColumnName
        {
            get { return addMonthColumnName; }
            set { SetProperty(ref addMonthColumnName, value); }
        }

        /// <summary>
        /// 计算基准年份，形如2020
        /// </summary>
        public int BaseYear
        {
            get { return baseYear; }
            set { SetProperty(ref baseYear, value); }
        }

        public ICommand PreciationCommand { get; set; }
        #endregion

        #region methods
        private bool Validation(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(FilePath))
                message = "文件路径不能为空！";
            if (!File.Exists(FilePath))
                message = "请确保文件存在！";
            if (BaseYear.ToString().Length != 4)
                message = "请填入正确格式的计算基准年份！形如2020";

            return message == string.Empty;
        }

        private void DoPreciationWork()
        {
            if(!Validation(out string message))
            {
                eventSource.Fire(EventSourceKeys.Depreciation_Valid, message);
                return;
            }

            var extension = Path.GetExtension(FilePath);
            IWorkbook workbook;

            // 解析excel，并计算
            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = NpoiHelper.GetWorkbookByExtension(extension, fs);

                // get sheet
                var sheets = new List<ISheet>();
                var sheetNames = SheetName?.Split(',');
                var numSheets = workbook.NumberOfSheets;
                if (sheetNames == null || sheetNames.Length == 0)
                    sheets = Enumerable.Range(0, numSheets).Select(index => workbook.GetSheetAt(index)).ToList();
                else
                    sheets = sheetNames.Select(name => workbook.GetSheet(name)).ToList();

                foreach (var sheet in sheets)
                {
                    // get headRow and column index
                    var headRow = sheet.GetRow(0);
                    var originalValueColumnIndex = -1;
                    var residualValueRateColumnIndex = -1;
                    var monthOfUseColumnIndex = -1;
                    var addMonthColumnIndex = -1;
                    foreach (var cell in headRow.Cells)
                    {
                        var index = headRow.Cells.IndexOf(cell);
                        if (cell == null)
                            continue;
                        if (cell.StringCellValue == OriginalValueColumnName)
                            originalValueColumnIndex = index;
                        if (cell.StringCellValue == ResidualValueRateColumnName)
                            residualValueRateColumnIndex = index;
                        if (cell.StringCellValue == MonthOfUseColumnName)
                            monthOfUseColumnIndex = index;
                        if (cell.StringCellValue == AddMonthColumnName)
                            addMonthColumnIndex = index;
                    }

                    if (originalValueColumnIndex == -1)
                    {
                        // TODO
                        eventSource.Fire(EventSourceKeys.Depreciation_Valid, $"sheet '{sheet.SheetName}'中未找到列名 '{OriginalValueColumnName}'");
                        continue;
                    }
                    if (residualValueRateColumnIndex == -1)
                    {
                        // TODO
                        eventSource.Fire(EventSourceKeys.Depreciation_Valid, $"sheet '{sheet.SheetName}'中未找到列名 '{ResidualValueRateColumnName}'");
                        continue;
                    }
                    if (monthOfUseColumnIndex == -1)
                    {
                        // TODO
                        eventSource.Fire(EventSourceKeys.Depreciation_Valid, $"sheet '{sheet.SheetName}'中未找到列名 '{MonthOfUseColumnName}'");
                        continue;
                    }
                    if (addMonthColumnIndex == -1)
                    {
                        // TODO
                        eventSource.Fire(EventSourceKeys.Depreciation_Valid, $"sheet '{sheet.SheetName}'中未找到列名 '{AddMonthColumnName}'");
                        continue;
                    }

                    for (int i = 1; i < sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                            continue;
                        var originalValueCell = row.GetCell(originalValueColumnIndex);
                        if (originalValueCell == null)
                            continue;
                        var residualValueRateCell = row.GetCell(residualValueRateColumnIndex);
                        if (residualValueRateCell == null)
                            continue;
                        var monthOfUseCell = row.GetCell(monthOfUseColumnIndex);
                        if (monthOfUseCell == null)
                            continue;
                        var addMonthCell = row.GetCell(addMonthColumnIndex);
                        if (addMonthCell == null)
                            continue;

                        // 平均值：(原币原值 * (100-净残值率)/100)/使用月限，默认保留两位小数
                        if (!double.TryParse(originalValueCell.ToString(), out var originalValue))
                            continue;
                        if (!double.TryParse(residualValueRateCell.ToString(), out var residualValueRate))
                            continue;
                        if (!double.TryParse(monthOfUseCell.ToString(), out var monthOfUse))
                            continue;
                        var addMonth = addMonthCell.ToString();
                        if (addMonth == null || addMonth.Length != 6)
                            continue;
                        var year = addMonth.Substring(0, 4);
                        if (!int.TryParse(year, out var yearValue))
                            continue;
                        var month = addMonth.Substring(4);
                        if (!int.TryParse(month,out var monthValue))
                            continue;
                        if (monthValue <=0 || monthValue > 12)
                            continue;
                        var average = Math.Round((originalValue * (100 - residualValueRate) / 100) / monthOfUse, 2);
                        // 如果计算基准年份是2020年，如果在2020之前，按照2020整年算；如果是202008，则算当月之后当年的月份
                        var resultCell = row.GetCell(row.LastCellNum + 1);
                        if (resultCell == null)
                            row.CreateCell(row.LastCellNum + 1);
                        if (yearValue < BaseYear)
                            resultCell.SetCellValue(average * 12);
                        else
                            resultCell.SetCellValue(average * (12 - monthValue));
                    }
                }


            }

            // 保存结果
            var resultFilePath = $"{FilePath.Substring(0, FilePath.IndexOf('.'))}{DateTime.Now.ToString("yyyyMMdd")}{Path.GetExtension(FilePath)}";
            //using (FileStream fileStream = File.Open(filepath,FileMode.Open, FileAccess.ReadWrite))
            using (FileStream fileStream = new FileStream(resultFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fileStream);
                // TODO
            }
        }
        #endregion
    }
}
