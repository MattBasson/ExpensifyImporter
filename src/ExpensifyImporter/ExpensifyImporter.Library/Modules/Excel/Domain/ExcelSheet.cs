using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Excel.Domain
{
    public class ExcelSheet : List<ExcelRow>
    {
        public ExcelSheet()
        {
        }

        public ExcelSheet(IEnumerable<ExcelRow> collection) : base(collection)
        {
        }

        public ExcelSheet(int capacity) : base(capacity)
        {
        }
    }
}
