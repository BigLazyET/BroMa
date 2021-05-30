using DoraPocket.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace DoraPocket.ViewModel
{
    public static class NpoiHelper
    {
        public static IWorkbook GetWorkbookByExtension(string extension, Stream stream)
        {
            Guard.ArgumentNotNull(extension, nameof(extension));
            Guard.ArgumentNotNull(stream, nameof(stream));

            IWorkbook workbook;
            switch (extension)
            {
                case ".xlsx":
                    workbook = new XSSFWorkbook(stream);
                    break;
                default:
                    workbook = new HSSFWorkbook(stream);
                    break;
            }
            return workbook;
        }
    }
}
