using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vnau.SyllabusParser.Core.Models
{
    public class Subject(string code, string name) : IComparable<Subject>
    {
        public string Code { get; set; } = code;

        public string Name { get; set; } = name;

        public int Index => GetIndex();

        public int CompareTo(Subject? other)
        {
            if (other == null) return 1;

            return Index - other.Index;
        }

        public override string ToString()
        {
            return $"{Code} {Name}";
        }

        private int GetIndex()
        {
            if (string.IsNullOrWhiteSpace(Code))
                return 0;

            if (Code == "n/a")
                return 0;

            return int.Parse(Code.Replace("ОК", string.Empty, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
