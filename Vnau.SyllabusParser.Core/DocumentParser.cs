//using DocumentFormat.OpenXml.Packaging;
//using System.Text.RegularExpressions;

//namespace Vnau.SyllabusParser.Core
//{
//    public class DocumentParser : IDisposable
//    {
//        private WordprocessingDocument _wordDocument;

//        public DocumentParser(string filePath)
//        {
//            _wordDocument = WordprocessingDocument.Open(filePath, false);
//        }



//        public Subject[] GetPreRequisites()
//        {
//            var paragraph = _wordDocument.MainDocumentPart?.Document.Body;

//            string[] prefixes = [
//                "Під час вивченні дисципліни можуть використовуватися знання",
//                "Під час вивчення даної дисципліни",
//                "Під час вивчення дисципліни можуть використовуватися знання",
//                "отримані з таких дисциплін",
//                "При вивченні даної дисципліни використовуються знання",
//                "При вивченні цієї дисципліни можуть використовуватись",
//                "При вивченні дисципліни можуть використовувати знання"
//            ];

//            var prefixSum = string.Join("|", prefixes);

//            var regex = new Regex($"({prefixSum}).*?(?<PreRequisites>(?:«[^.]*?»[,\\sтаі]*)+)", RegexOptions.IgnoreCase);
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();

//            var match = regex.Match(text);
//            if (match.Length == 0)
//            {
//                return [];
//            }

//            var preRequisitesString = match.Groups["PreRequisites"].Value;
//            var subjects = preRequisitesString.Split([",", "«", "»"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

//            return subjects.Where(s => s != "та").Select(ToSubject).OrderBy(s => s).ToArray();
//        }

//        public Subject[] GetPostRequisites()
//        {
//            string[] prefixes = [
//                "Основні положення навчальної дисципліни можуть застосовуватися",
//                "застосовувати мовний матеріал при вивченні дисципліни",
//                "дисципліни мають застосовуватися при вивченні таких дисциплін",
//                "дисципліни мають застосовуватися при вивченні дисципліни",
//                "дисципліни можуть  застосовуватися при вивченні таких дисциплін"
//            ];

//            var prefixSum = string.Join("|", prefixes);

//            var regex = new Regex($"({prefixSum}).*?(?<PostRequisites>(?:«[^.]*?»[,\\sтаі]*)+)", RegexOptions.IgnoreCase);
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();

//            var match = regex.Match(text);
//            if (match.Length == 0)
//            {
//                return [];
//            }

//            var preRequisitesString = match.Groups["PostRequisites"].Value;
//            var subjects = preRequisitesString.Split([",", "«", "»"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

//            return subjects.Select(ToSubject).OrderBy(s => s).ToArray();
//        }

//        public int EducationYear()
//        {
//            var regex = new Regex("Рік навчання.*?(?<Year>\\d)-й", RegexOptions.IgnoreCase);
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();

//            var educationYear = regex.Match(text).Groups["Year"].Value;
//            if (educationYear == null)
//            {
//                return 0;
//            }

//            return int.Parse(educationYear);
//        }

//        public int[] Semesters()
//        {
//            var regex = new Regex("(?<=семестр).*(?=Кількість кредитів)", RegexOptions.IgnoreCase);
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();
//            var numberRegex = new Regex("\\d");

//            var match = regex.Match(text);
//            if (match.Length == 0)
//            {
//                regex = new Regex("\\d[\\s-]семестр", RegexOptions.IgnoreCase);
//                match = regex.Match(text);
//            }

//            var semestersString = match.Value;
//            var semesters = numberRegex.Matches(semestersString)
//                .Select(s => s.Value)
//                .Select(int.Parse);


//            return semesters.ToArray();
//        }

//        public Subject ParseSubjectFromName()
//        {
//            var regex = new Regex("СИЛАБУС\\s?НАВЧАЛЬНОЇ ДИСЦИПЛІНИ«(?<name>.*?)»", RegexOptions.IgnoreCase);
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();

//            var name = regex.Match(text).Groups["name"].Value;
//            return ToSubject(name);
//        }

//        private Subject ToSubject(string subjectName)
//        {
//            static bool namesComparer(string nameA, string nameB)
//            {
//                if (nameA.Equals(nameB, StringComparison.CurrentCultureIgnoreCase))
//                {
//                    return true;
//                }

//                nameA = nameA.Replace("та переробки ", "", StringComparison.InvariantCultureIgnoreCase);
//                nameB = nameB.Replace("та переробки ", "", StringComparison.InvariantCultureIgnoreCase);

//                string[] remove = [" та ", " і ", "'", "’", "ʼ"];
//                foreach (string item in remove)
//                {
//                    nameA = nameA.Replace(item, string.Empty, StringComparison.InvariantCultureIgnoreCase);
//                    nameB = nameB.Replace(item, string.Empty, StringComparison.InvariantCultureIgnoreCase);
//                }

//                nameA = nameA.Replace("проектування", "проєктування", StringComparison.InvariantCultureIgnoreCase);
//                nameB = nameB.Replace("проектування", "проєктування", StringComparison.InvariantCultureIgnoreCase);

//                nameA = nameA.Replace("культорологія", "культурологія", StringComparison.InvariantCultureIgnoreCase);
//                nameB = nameB.Replace("культорологія", "культурологія", StringComparison.InvariantCultureIgnoreCase);

//                nameA = nameA.Replace("життєдвяльності", "життєдіяльності", StringComparison.InvariantCultureIgnoreCase);
//                nameB = nameB.Replace("життєдвяльності", "життєдіяльності", StringComparison.InvariantCultureIgnoreCase);

//                nameA = nameA.Replace(" (Energy efficiency and alternative energy sources)", "", StringComparison.InvariantCultureIgnoreCase);
//                nameB = nameB.Replace(" (Energy efficiency and alternative energy sources)", "", StringComparison.InvariantCultureIgnoreCase);

//                return nameA.Equals(nameB, StringComparison.CurrentCultureIgnoreCase);
//            }

//            var knownSubject = SubjectsKnownData.Subjects.FirstOrDefault(s => namesComparer(s.Name, subjectName));
//            if (knownSubject != null)
//            {
//                return knownSubject;
//            }
//            var text = (_wordDocument.MainDocumentPart?.Document?.Body?.InnerText) ?? throw new Exception();

//            return new Subject("n/a", subjectName);
//        }

//        public void Dispose()
//        {
//            _wordDocument.Dispose();
//        }
//    }
//}
