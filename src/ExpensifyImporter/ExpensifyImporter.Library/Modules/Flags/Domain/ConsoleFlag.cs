using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Flags.Domain
{
    public record ConsoleFlag(FlagType FlagType, string[] Variations);
}
