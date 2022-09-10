using ExpensifyImporter.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Application
{
    internal record SupportedFlag(Flags Flag, string FlagValue, string OriginalFlag);
    internal class FlagExtractor
    {
        private readonly string[] args;

        public FlagExtractor(string[] args)
        {
            this.args = args;
        }

        public List<SupportedFlag> GetSupportedFlags() {
            var consoleFlags = new List<SupportedFlag>();
            if (this.args.Length > 0)
            {
                var argumentSuppliedFlags = args
                    .Where(w => w.Contains("--") || w.Contains("-"))
                    .Select((s, i) => new { Index = i, Flag = s.Replace("-", ""), FlagValue = args.Length < i + 1 ? args[i + 1] : "" });

                if (!argumentSuppliedFlags.Any())
                {
                    Console.WriteLine("Please specify an argument or flag or use help -h or -help");
                    return consoleFlags;
                }

                var supportedFlags = DataHelper.ConsoleFlags;
                var flags = argumentSuppliedFlags
                    .Where(w => supportedFlags.Any(a => a.Variations.Contains(w.Flag)))
                    .Select(s => {
                        var supportedFlag = supportedFlags.FirstOrDefault(a => a.Variations.Contains(s.Flag));
                        if (supportedFlag != null)
                        {
                            return new SupportedFlag(supportedFlag.FlagType, s.FlagValue, s.Flag);
                        }
                        else
                        {
                            return new SupportedFlag(Flags.Unsupported, s.Flag, s.FlagValue);
                        }                        
                    });
                
                consoleFlags.AddRange(flags);
                return consoleFlags;

            }


            Console.WriteLine("Please specify an argument or flag or use help -h or --help");
            return consoleFlags;

        }
    }        

    
}

