using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
namespace ExcelToBDTGSqlite
{
    public class Kitchen
    {
        public long ChatId;
        private string typeFace;
        private string typeTable;
        private int lenght;
        private int basePrice;
        private int totalPrice;

        public string TypeFace
        {
            get { return typeFace; }
            set { typeFace = value; }
        }
        public string TypeTable
        {
            get { return typeTable; }
            set { typeTable = value; }
        }

        public int Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }

        public Kitchen(string Face, string Table, int Lenght)
        {
            typeFace = Face;
            typeTable = Table;
            lenght = Lenght;
        }
        public Kitchen()
        {
            ChatId = -1;
            typeFace = "null";
            typeTable = "null";
            lenght = 0;
        }

        private void FindBasePriceXLSX(string path)
        {
            FileInfo File = new FileInfo(path);
            using (ExcelPackage pck = new ExcelPackage(File))
            {
                var format = new ExcelTextFormat();
                format.Delimiter = '/';
                ExcelWorksheet ws = null;
                ws = pck.Workbook.Worksheets[1];

                var faceRow =
                from cell in ws.Cells["a:m"]
                where cell.Value?.ToString() == typeFace
                select cell;

                var tableRow =
                from cell in ws.Cells["a:m"]
                where cell.Value?.ToString() == typeTable
                select cell;

                basePrice = int.Parse(ws.Cells[tableRow.ToList()[0].Start.Row, faceRow.ToList()[0].Start.Column].Value.ToString());

            }
        }

        public int GetFullPrice(string path)
        {
            FindBasePriceXLSX(path);
            totalPrice = basePrice * lenght;

            return totalPrice;
        }
        public int GetSalePrice(string path, double sale)
        {
            FindBasePriceXLSX(path);
            totalPrice = basePrice * lenght;

            return int.Parse((totalPrice / sale).ToString());
        }

    }
}
