namespace DoraPocket.ViewModel.Depreciation
{
    public class DepreciationViewModel : BaseViewModel
    {
        private string filePath;
        private string sheetName;
        private double originalValue;
        private double residualValueRate;

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
        /// 若为空，则默认全部sheet都进行计算；若非空，多个待计算的sheet用英文','分隔
        /// </summary>
        public string SheetName
        {
            get { return sheetName; }
            set { SetProperty(ref sheetName, value); }
        }



    }
}
