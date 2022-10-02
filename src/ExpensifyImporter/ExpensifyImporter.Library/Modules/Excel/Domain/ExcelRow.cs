using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Excel.Domain
{
    public class ExcelRow : List<ExcelCell>
    {
        public ExcelRow()
        {
        }

        public ExcelRow(IEnumerable<ExcelCell> collection) : base(collection)
        {
        }

        public ExcelRow(int capacity) : base(capacity)
        {
        }

        
        public ExcelCell this[string col] => this.FirstOrDefault(f => f.ColumnReference == col);


    }
}
