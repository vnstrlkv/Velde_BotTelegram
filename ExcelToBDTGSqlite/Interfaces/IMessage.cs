using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelToBDTGSqlite.Models.Interfaces
{
    public interface IMessage
    {
        string Message { get; set; }
        int Stage { get; set; }
        
    }
}
