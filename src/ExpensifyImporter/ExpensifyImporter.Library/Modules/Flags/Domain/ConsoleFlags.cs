using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Flags.Domain
{
    public static class ConsoleFlags
    {
        public static List<ConsoleFlag> PermittedConsoleFlags = new List<ConsoleFlag>()
        {
            new ConsoleFlag(FlagType.Help, new string[] { "help", "h" }),
            new ConsoleFlag(FlagType.Directory, new string[] { "directory", "dir", "d"})
        };
    }
}
