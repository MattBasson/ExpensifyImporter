using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ExpensifyImporter.Library.Modules.IO
{
    public static class EmbeddedData
    {
        public static string Get(string objectPath)
        {
            
            var assembly = Assembly.GetExecutingAssembly();
            return Get(objectPath, assembly);
        }

        public static string Get(string objectPath, Assembly assembly)
        {
            using var stream =
               assembly.GetManifestResourceStream(objectPath);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        
        public static async Task<byte[]?> GetByteArrayAsync(string objectPath, Assembly assembly)
        {
            await using var stream =
                assembly.GetManifestResourceStream(objectPath);
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            using var memoryStream = new MemoryStream();
            await stream?.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        
        
    }
}
