namespace Vnau.SyllabusParser.Core.Models
{
    internal record DocxFileData
    {
        public required List<string> SubjectDescription { get; init; }
        public required List<DocxFileSection> Sections { get; init; }
    }

    internal record DocxFileSection
    {
        public required string Name { get; set; }
        public required List<string> Content { get; init; }
    }
}
