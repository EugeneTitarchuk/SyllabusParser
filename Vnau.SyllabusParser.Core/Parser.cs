using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.QueryParsers.Classic;
using Vnau.SyllabusParser.Core.Models;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Search.VectorHighlight;
using Lucene.Net.QueryParsers.Surround.Query;
using Lucene.Net.Search.Highlight;
using System.Formats.Tar;
using System.Text;

using static Lucene.Net.Search.VectorHighlight.FieldPhraseList;
using static Lucene.Net.Search.VectorHighlight.FieldTermStack;
using static Lucene.Net.Search.VectorHighlight.FieldFragList;

namespace Vnau.SyllabusParser.Core
{
    public class Parser
    {
        private readonly string[] _knownSubjects;
        private readonly string[] _knownSections;

        public Parser(string[] knownSubjects, string[] knownSections)
        {
            _knownSubjects = knownSubjects;
            _knownSections = knownSections;
        }


        public SyllabusInfo? ParseFile(string syllabusPath)
        {
            var docxParser = new DocxFileParser(_knownSections);

            var docxParseResults = docxParser.Parse(syllabusPath);

            var luceneDocs = LuceneDocumentBuilder.BuildDocuments(docxParseResults);

            var knownSubjects = SubjectsKnownData.ParseKnownSubjects(_knownSubjects);
            foreach (var knownSubject in knownSubjects)
            {
                var searchResults = SearchSubject(luceneDocs, knownSubject.Name);

                //foreach (var searchResult in searchResults.ScoreDocs)
                //{

                //}
            }


            return null;
        }

        private ScoreDoc[] SearchSubject(IEnumerable<Document> luceneDocs, string subjectName)
        {
            var luceneVersion = LuceneVersion.LUCENE_48;
            var standardAnalyzer = new StandardAnalyzer(luceneVersion);
            
            using var directory = new SimpleFSDirectory(AppContext.BaseDirectory);
            var config = new IndexWriterConfig(luceneVersion, standardAnalyzer);
            using var indexWriter = new IndexWriter(directory, config);
            indexWriter.DeleteAll();
            indexWriter.AddDocuments(luceneDocs);
            indexWriter.Flush(triggerMerge: false, applyAllDeletes: false);

            FuzzyQuery field1Query = new(new Term("content", subjectName), 2, 0, 50, transpositions: true);

            var queryParser = new QueryParser(luceneVersion, "content", standardAnalyzer);
            Query query = queryParser.Parse($"\"українська мова та етнокультуролsгія\"~2");

            using var reader = indexWriter.GetReader(applyAllDeletes: true);
            var searcher = new IndexSearcher(reader);
            var hits = searcher.Search(query, n: 20).ScoreDocs;

            var vectorHighlighter = new FastVectorHighlighter(
                phraseHighlight: true,
                fieldMatch: true,
                new SimpleFragListBuilder(),
                new ScoreOrderFragmentsBuilder(["start_position"], ["end_position"])
            );

            foreach (var hit in hits)
            {
                var fieldName = "content";

                var fieldQuery = vectorHighlighter.GetFieldQuery(query);

                var fieldTermStack = new FieldTermStack(reader, hit.Doc, fieldName, fieldQuery);
                var fieldPhraseList = new FieldPhraseList(fieldTermStack, fieldQuery);

                foreach (var fieldPhrase in fieldPhraseList.PhraseList)
                {
                    int phraseStartOffset = fieldPhrase.StartOffset;
                    int phraseEndOffset = fieldPhrase.EndOffset;

                    foreach (var termInfo in fieldPhrase.TermsInfos)
                    {
                        var text = termInfo.Text;
                        int termPosition = termInfo.Position + 1;
                        int termStartOffset = termInfo.StartOffset;
                        int termEndOffset = termInfo.EndOffset;
                    }
                }
            }

            return hits;
        }

        private void HandleHit()
        {

        }

        //public static SyllabusInfo Parse(string syllabusPath)
        //{
        //    using var document = new DocumentParser(syllabusPath);


            

        //    var syllabus = new SyllabusInfo();

        //    syllabus.Subject = document.ParseSubjectFromName();
        //    syllabus.PreRequisites = document.GetPreRequisites();
        //    syllabus.PostRequisites = document.GetPostRequisites();
        //    syllabus.EducationYear = document.EducationYear();
        //    syllabus.Semesters = document.Semesters();

        //    return syllabus;
        //}
    }
}
