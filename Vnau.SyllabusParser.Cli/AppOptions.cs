namespace Vnau.SyllabusParser.Cli
{
    public class AppOptions
    {
        public string? InputFolder { get; set; }

        public string? OutputFile { get; set; }

        public string[] KnownSubjects { get; set; } = [];
        public string[] SectionNames { get; set; } = [];

        public static Dictionary<string, string> CliAliases => new() 
        { 
            { "-i", nameof(InputFolder) },
            { "-o", nameof(OutputFile) }, 
            { "--in", nameof(InputFolder) },
            { "--out", nameof(OutputFile) }
        };
    }
}
