using DoraPocket.Common;
using DoraPocket.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace DoraPocket.ViewModel.Induction
{
    public class InductionViewModel : BaseViewModel
    {
        #region fields
        private string ruleFilePath;
        private string inductionFilePath;
        private int startRow;
        private int inductionColumn;
        private string sheetIndexes;
        private readonly IDictionary<string, string> rules;

        private readonly IEventSource eventSource;
        #endregion

        #region properties
        /// <summary>
        /// 归纳规则文件路径
        /// </summary>
        public string RuleFilePath
        {
            get { return ruleFilePath; }
            set { SetProperty(ref ruleFilePath, value); }
        }

        /// <summary>
        /// 待归纳文件路径
        /// </summary>
        public string InductionFilePath
        {
            get { return inductionFilePath; }
            set { SetProperty(ref inductionFilePath, value); }
        }

        /// <summary>
        /// 从哪行起开始归纳，不填默认从第0行开始归纳
        /// </summary>
        public int StartRow
        {
            get { return startRow; }
            set { SetProperty(ref startRow, value); }
        }

        /// <summary>
        /// 对哪一列进行归纳
        /// </summary>
        public int InductionColumn
        {
            get { return inductionColumn; }
            set { SetProperty(ref inductionColumn, value); }
        }

        /// <summary>
        /// 需要进行归纳的sheets
        /// </summary>
        public string SheetIndexes
        {
            get { return sheetIndexes; }
            set { SetProperty(ref sheetIndexes, value); }
        }

        public ICommand InductionCommand { get; set; }
        #endregion

        #region construct
        public InductionViewModel()
        {
            rules = new Dictionary<string, string>();
            eventSource = ServiceProviderAccessor.Current.GetRequiredService<IEventSource>();
            InductionCommand = new BaseCommand(_ => DoInductionWork());
        }
        #endregion

        #region methods
        private bool Validation(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(RuleFilePath))
                message = "归纳规则文件路径不能为空！";
            if (!File.Exists(RuleFilePath))
                message = "请确保 归纳规则文件存在！";
            if (string.IsNullOrWhiteSpace(InductionFilePath))
                message = "待归纳文件路径不能为空！";
            if (!File.Exists(InductionFilePath))
                message = "请确保待归纳文件存在！";
            if (StartRow < 0)
                message = "归纳起始行设置必须大于等于1！";
            if (InductionColumn < 0)
                message = "归纳列设置必须大于等于1！";
            if (!string.IsNullOrWhiteSpace(SheetIndexes))
            {
                var sheetIndexes = SheetIndexes.Split(',');
                foreach (var sheetIndex in sheetIndexes)
                {
                    if (!int.TryParse(sheetIndex, out var index))
                    {
                        message = $"{sheetIndex}不是整数，请填写整数值并确保多个时用英文逗号隔开！";
                        break;
                    }
                    if (index <= 0)
                    {
                        message = $"{sheetIndex}只能大于等于1！";
                        break;
                    }
                }
            }

            return message == string.Empty;
        }

        private void DoInductionWork()
        {
            if (!Validation(out var message))
            {
                eventSource.Fire(EventSourceKeys.Induction_Valid, message);
                return;
            }

            // 取归纳规则
            using (var fs = File.Open(RuleFilePath, FileMode.Open, FileAccess.Read))
            {
                var extension = Path.GetExtension(RuleFilePath);
                IWorkbook workbook = NpoiHelper.GetWorkbookByExtension(extension, fs);

                // 默认取第一个sheet的前两列形成归纳规则字典
                var sheet = workbook.GetSheetAt(0);

                var enumerator = sheet.GetRowEnumerator();
                while (enumerator.MoveNext())
                {
                    var row = (IRow)enumerator.Current;
                    if (row == null)
                        // TODO
                        continue;
                    var keyCell = row.GetCell(0);
                    if (keyCell == null || string.IsNullOrWhiteSpace(keyCell.ToString()))
                        // TODO
                        continue;
                    var valueCell = row.GetCell(1);
                    if (valueCell == null || string.IsNullOrWhiteSpace(valueCell.ToString()))
                        // TODO
                        continue;

                    rules[keyCell.ToString()] = valueCell.ToString();
                }
            }

            if (rules == null || !rules.Any())
            {
                // TODO
                return;
            }

            // 归纳规则 去 匹配
            IWorkbook wb;
            using (var fs = File.Open(InductionFilePath, FileMode.Open, FileAccess.Read))
            {
                var extension = Path.GetExtension(InductionFilePath);
                wb = NpoiHelper.GetWorkbookByExtension(extension, fs);

                var sheets = Enumerable.Range(0, wb.NumberOfSheets);
                if (!string.IsNullOrWhiteSpace(SheetIndexes))
                {
                    sheets = SheetIndexes.Split(',').Select(x => int.Parse(x));
                }

                // 匹配需要匹配的sheet
                foreach (var sheetIndex in sheets)
                {
                    var sheet = wb.GetSheetAt(sheetIndex);
                    if (sheet == null)
                    {
                        // TODO
                        continue;
                    }
                    var startRow = StartRow == 0 ? 0 : StartRow - 1;
                    var column = InductionColumn == 0 ? 0 : InductionColumn - 1;
                    for (int i = startRow; i < sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            // TODO
                            continue;
                        }
                        
                        var contentCell = row.GetCell(column);
                        if (contentCell == null)
                        {
                            // TODO
                            continue;
                        }
                        var content = contentCell.ToString();
                        var value = rules.FirstOrDefault(rule => content.Contains(rule.Key)).Value;
                        if (value != null)
                        {
                            var resultCell = row.GetCell(row.LastCellNum + 1);
                            if (resultCell == null)
                                row.CreateCell(row.LastCellNum + 1);
                            resultCell.SetCellValue(value);
                        }
                    }
                }
            }

            // 保存结果
            var resultFilePath = $"{InductionFilePath.Substring(0, InductionFilePath.IndexOf('.'))}{DateTime.Now.ToString("yyyyMMdd")}{Path.GetExtension(InductionFilePath)}";
            //using (FileStream fileStream = File.Open(filepath,FileMode.Open, FileAccess.ReadWrite))
            using (FileStream fileStream = new FileStream(resultFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                wb.Write(fileStream);
                // TODO
            }
        }
        #endregion
    }
}
