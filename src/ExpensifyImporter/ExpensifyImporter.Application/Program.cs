// See https://aka.ms/new-console-template for more information
using ExpensifyImporter.Application;
using ExpensifyImporter.Application.Data;

var flagExtractor = new FlagExtractor(args);

var flags = flagExtractor.GetSupportedFlags();

if (!flags.Any())
{
    Console.WriteLine("Please specify an argument or flag or use help -h or -help");
    Console.Read();
}

if(flags.Any(a=>a.Flag == Flags.Help))
{
    Console.WriteLine(DataHelper.Get("ExpensifyImporter.Application.Data.Content.HelpContent.txt"));
    Console.Read();
}

if(flags.Any(a=>a.Flag == Flags.Directory))
{
    //Directory flag specified processing can begin and resource newing up can start.

    

}   


