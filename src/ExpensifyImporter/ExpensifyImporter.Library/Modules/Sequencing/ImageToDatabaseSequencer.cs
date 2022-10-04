using ExpensifyImporter.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Sequencing
{
    public class ImageToDatabaseSequencer
    {
        public ImageToDatabaseSequencer(
            ILogger<ImageToDatabaseSequencer> logger,
            ExpensifyContext dbContext)
        {
        }

        public async Task<int> ProcessAsync(int batchSize = 0)
        {
            // 1) Get dataset of items that have no image set (batch size sensitive)
            //ExpenseImageBatchQuery returns ExpenseID array


            // 2) Download images  return an array of ExpenseIds and byte arrays.
            //Image downloader
            
            // 3) Asynchronous saving of array
            //return rows modifed
            //Expensse Batch Commaand
            

            return 0;
        }
    }
}
