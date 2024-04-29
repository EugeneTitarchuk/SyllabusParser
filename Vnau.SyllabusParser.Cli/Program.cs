using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using Vnau.SyllabusParser.Core;

namespace Vnau.SyllabusParser.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false)
                .AddCommandLine(args, AppOptions.CliAliases)
                .Build();

            var options = config.Get<AppOptions>() ?? throw new Exception("Cannot read application options");


            using var writer = new StreamWriter(File.Create("result.txt"));

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var fileEntries = Directory.GetFiles("D:\\temp\\opp\\ГОТО 15.04");
            fileEntries.OrderBy(s => s);

            var parser = new Parser(options.KnownSubjects, options.SectionNames);
            foreach (var entry in fileEntries)
            {
                parser.ParseFile(entry);
            }

            //var syllabusesList = fileEntries.Select(Parser.Parse).OrderBy(s => s.Subject);

            //foreach (var item in syllabusesList)
            //{
            //    Console.WriteLine(item);

            //    writer.WriteLine(item);
            //}

        }
    }
}
