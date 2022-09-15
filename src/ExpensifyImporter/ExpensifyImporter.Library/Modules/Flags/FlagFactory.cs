using ExpensifyImporter.Library.Modules.EmbeddedData;
using ExpensifyImporter.Library.Modules.Flags.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Flags
{
    
    public class FlagFactory
    {
        private readonly string[] args;

        public FlagFactory(string[] args)
        {
            this.args = args;
        }

        public List<SupportedFlag> GetSupportedFlags()
        {
            var consoleFlags = new List<SupportedFlag>();
            if (args.Length > 0)
            {
                var argumentSuppliedFlags = args
                    .Where(w => w.Contains("--") || w.Contains("-"))
                    .Select((s, i) => new { Index = i, Flag = s.Replace("-", ""), FlagValue = args.Length < i + 1 ? args[i + 1] : "" });

                if (!argumentSuppliedFlags.Any())
                {
                    Console.WriteLine("Please specify an argument or flag or use help -h or -help");
                    return consoleFlags;
                }

                var supportedFlags = ConsoleFlags.PermittedConsoleFlags;
                var flags = argumentSuppliedFlags
                    .Where(w => supportedFlags.Any(a => a.Variations.Contains(w.Flag)))
                    .Select(s =>
                    {
                        var supportedFlag = supportedFlags.FirstOrDefault(a => a.Variations.Contains(s.Flag));
                        if (supportedFlag != null)
                        {
                            return new SupportedFlag(supportedFlag.FlagType, s.FlagValue, s.Flag);
                        }
                        else
                        {
                            return new SupportedFlag(FlagType.Unsupported, s.Flag, s.FlagValue);
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

