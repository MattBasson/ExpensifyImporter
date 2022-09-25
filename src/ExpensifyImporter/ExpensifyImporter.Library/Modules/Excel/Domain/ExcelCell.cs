using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Excel.Domain
{
    public record ExcelCell(int Index,string? CellValue);
}
