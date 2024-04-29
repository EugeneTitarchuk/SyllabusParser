using Vnau.SyllabusParser.Core.Models;
using FuzzySharp;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;

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


            var a = Process.ExtractOne("етнокультурологjя".ToUpper(), docxParseResults.SubjectDescription, s => s, ScorerCache.Get<DefaultRatioScorer>());




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
