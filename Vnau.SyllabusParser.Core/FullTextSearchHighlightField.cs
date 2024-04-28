using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Vnau.SyllabusParser.Core
{
    public sealed class FullTextSearchHighlightField : Field
    {
        public static readonly FieldType TYPE = LoadType();

        private static FieldType LoadType()
        {
            var fieldType = new FieldType
            {
                //Full-Text Search
                IsIndexed = true,
                IsTokenized = true,
                OmitNorms = false,
                //Highlight
                IsStored = true,
                StoreTermVectors = true,
                StoreTermVectorPositions = true,
                StoreTermVectorOffsets = true,
                StoreTermVectorPayloads = true,
                IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS
            };
            fieldType.Freeze();
            return fieldType;
        }

        public FullTextSearchHighlightField(string name, string value)
            : base(name, value, TYPE)
        {
        }
    }
}
