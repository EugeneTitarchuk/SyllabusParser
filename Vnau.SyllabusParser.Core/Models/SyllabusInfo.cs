using System.Text;

namespace Vnau.SyllabusParser.Core.Models
{
    public class SyllabusInfo
    {
        public Subject? Subject { get; set; }
        public int EducationYear { get; set; }
        public int[]? Semesters { get; set; }
        public Subject[] PreRequisites { get; set; } = [];
        public Subject[] PostRequisites { get; set; } = [];
        public string? GeneralCompetences { get; set; }
        public string? SpecialCompetences { get; set; }
        public string? ProgramResults { get; set; }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine(Subject?.ToString());
            strBuilder.AppendLine($"\tYear: {EducationYear}. Semesters: {string.Join(", ", Semesters ?? [])}");
            strBuilder.AppendLine($"\tPreRequisites:");
            foreach (var item in PreRequisites)
            {
                strBuilder.AppendLine($"\t\t{item}");
            }
            strBuilder.AppendLine($"\tPostRequisites:");
            foreach (var item in PostRequisites)
            {
                strBuilder.AppendLine($"\t\t{item}");
            }

            return strBuilder.ToString();
        }
    }
}
