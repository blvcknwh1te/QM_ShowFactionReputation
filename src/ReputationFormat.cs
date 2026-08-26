using System.Globalization;

namespace ShowFactionReputation
{
    public static class ReputationFormat
    {
        public static string Format(float reputation)
        {
            return reputation.ToString("0.#", CultureInfo.InvariantCulture);
        }

        public static string AppendToTechLevel(string techLevelText, float reputation)
        {
            if (string.IsNullOrEmpty(techLevelText))
                return Format(reputation);

            return $"{techLevelText} · {Format(reputation)}";
        }
    }
}
