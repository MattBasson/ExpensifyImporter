using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Domain
{
    public class FeatureConfiguration
    {
        /// <summary>
        /// Watches the directory and responds to the event created when a new file is added.
        /// </summary>
        public bool WatchDirectory { get; set; }

        /// <summary>
        /// Polls the directory by the Workers interval and loads any new files found
        /// </summary>
        public bool PollDirectory { get; set; }
    }
}
