using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Application.Data
{
    internal record ConsoleFlag(Flags FlagType, string[] Variations);


    internal static class DataHelper
    {
        public static string Get(string objectPath)
        {
            var assembly  = Assembly.GetExecutingAssembly();
            using var stream =
                assembly.GetManifestResourceStream(objectPath);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static List<ConsoleFlag> ConsoleFlags = new List<ConsoleFlag>() 
        { 
            new ConsoleFlag(Flags.Help, new string[] { "help", "h" }),
            new ConsoleFlag(Flags.Directory, new string[] { "directory", "dir", "d"})
        };
    }
}
