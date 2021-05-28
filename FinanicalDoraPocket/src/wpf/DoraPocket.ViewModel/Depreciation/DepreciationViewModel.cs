using System;

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
        public string OriginalValue
        {
            get { return originalValueColumnName; }
            set { SetProperty(ref originalValueColumnName, value); }
        }

        /// <summary>
        /// 列名：净残值率%，其对应的cell里的值形如5
        /// </summary>
        public string ResidualValueRate
        {
            get { return residualValueRateColumnName; }
            set { SetProperty(ref residualValueRateColumnName, value); }
        }

        /// <summary>
        /// 列名：使用月限，其对应的cell里的值形如12
        /// </summary>
        public string MonthOfUse
        {
            get { return monthOfUseColumnName; }
            set { SetProperty(ref monthOfUseColumnName, value); }
        }

        /// <summary>
        /// 列名：增加月份，其对应的cell里的值形如202008
        /// </summary>
        public string AddMonth
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
        #endregion

        #region methods
        private bool Validation()
        {
            return true;
        }
        #endregion
    }
}
