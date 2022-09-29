
namespace ExpensifyImporter.Library.Domain
{
    public class WorkerConfiguration
    {
        
        public int Interval { get; set; }

        public string DataDirectory { get; set; }

        public string ExpensifyAuthToken { get; set; }
    }
}
