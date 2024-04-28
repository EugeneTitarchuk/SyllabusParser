using Lucene.Net.Documents;
using Vnau.SyllabusParser.Core.Models;

namespace Vnau.SyllabusParser.Core
{
    internal class LuceneDocumentBuilder
    {
        public static IEnumerable<Document> BuildDocuments(DocxFileData docxFileData)
        {
            var description = string.Join(Environment.NewLine, docxFileData.SubjectDescription);

            yield return new Document
            {
                new TextField("title", "description", Field.Store.YES),
                new FullTextSearchHighlightField("content", description)
            };

            foreach (var section in docxFileData.Sections)
            {
                var sectionContent = string.Join(Environment.NewLine, section.Content);

                yield return new Document
                {
                    new TextField("title", section.Name, Field.Store.YES),
                    new FullTextSearchHighlightField("content", sectionContent)
                };
            } 
        }
    }
}
