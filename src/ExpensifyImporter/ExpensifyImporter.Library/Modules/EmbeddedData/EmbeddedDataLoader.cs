using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.EmbeddedData
{    
    public static class EmbeddedDataLoader
    {
        public static string Get(string objectPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream =
                assembly.GetManifestResourceStream(objectPath);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }        
    }
}
