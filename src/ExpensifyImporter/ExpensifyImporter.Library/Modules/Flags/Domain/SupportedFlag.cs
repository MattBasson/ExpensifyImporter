using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Flags.Domain
{
    public record SupportedFlag(FlagType Flag, string FlagValue, string OriginalFlag);
}
