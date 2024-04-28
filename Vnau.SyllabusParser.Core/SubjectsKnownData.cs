using Vnau.SyllabusParser.Core.Models;

namespace Vnau.SyllabusParser.Core
{
    internal class SubjectsKnownData
    {
        public static Subject[] ParseKnownSubjects(string[] subjects)
        {
            return subjects.Select(StringToSubject).ToArray();
        }

        private static Subject StringToSubject(string subject)
        {
            var parts = subject.Split([' '], 2);
            return new Subject(parts[0], parts[1]);
        }
    }
}
