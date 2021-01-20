using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
namespace VeldeBotTelegram.Models
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
            lenght = -1;
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
                from cell in ws.Cells["a:aa"]
                where cell.Value?.ToString() == typeFace
                select cell;

                var tableRow =
                from cell in ws.Cells["a:aa"]
                where cell.Value?.ToString() == typeTable
                select cell;
               int i =tableRow.ToList()[0].Start.Row;
                int j= faceRow.ToList()[0].Start.Column;
                string tmp = ws.Cells[i, j].Value.ToString();
                
                basePrice = (int)double.Parse(tmp);

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


            return (int)(totalPrice / sale);
        }

    }
}
