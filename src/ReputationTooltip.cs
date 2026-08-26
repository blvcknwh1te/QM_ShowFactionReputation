using System.Globalization;
using MGSC;

namespace ShowFactionReputation
{
    /// <summary>SSOT: подпись, значение и цвет строки репутации в тултипах.</summary>
    internal static class ReputationTooltip
    {
        public const string LocKey = "tooltip.reputation";
        public const string IconTag = "common_reputation";

        // Fallback: тот же смысл в локали игры, если основной ключ не резолвится.
        private static readonly string[] LabelKeys =
        {
            LocKey,
            "ui.tradeshuttle.mode.reputation",
        };

        private static TooltipProperty _pendingColorTarget;

        public static string LocalizedLabel()
        {
            foreach (string key in LabelKeys)
            {
                string text = Localization.Get(key, warnIfMissingTag: false);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }

            return Localization.Get(LocKey);
        }

        public static void ApplySignColors(TooltipProperty panel, float reputation)
        {
            if (panel == null)
                return;

            if (reputation < 0f)
            {
                panel.SetValueColor(Colors.LightRed);
                panel.SetNameColor(Colors.LightRed);
            }
            else if (reputation > 0f)
            {
                // Green: читаемый плюс; AltGreen сливается с дефолтным teal подписей.
                panel.SetValueColor(Colors.Green);
                panel.SetNameColor(Colors.Green);
            }
        }

        public static bool TryParseLeadingReputation(string value, out float reputation)
        {
            reputation = 0f;
            if (string.IsNullOrEmpty(value))
                return false;

            int i = 0;
            if (value[0] == '+' || value[0] == '-')
                i = 1;

            int start = i;
            while (i < value.Length && char.IsDigit(value[i]))
                i++;

            if (i == start)
                return false;

            return float.TryParse(
                value.Substring(0, i),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out reputation);
        }

        public static void AddMissionRow(TooltipFactory factory, float reputation)
        {
            TooltipProperty panel = factory.AddPanelToTooltip();
            panel.SetIcon(IconTag);
            // SetName сам делает FirstLetterToUpperCase — не через LocalizeName(ключ).
            panel.SetName(LocalizedLabel());
            panel.SetValue(FormatHelper.ToInt(reputation, showPlus: true), firstLetterToUpperCase: false);
            ApplySignColors(panel, reputation);
        }

        public static void NoteLocalizedName(TooltipProperty panel, string tag)
        {
            if (panel != null && tag == LocKey)
                _pendingColorTarget = panel;
        }

        public static void ApplyColorsIfPending(TooltipProperty panel, string value)
        {
            if (panel == null || panel != _pendingColorTarget)
                return;

            _pendingColorTarget = null;
            if (!TryParseLeadingReputation(value, out float reputation))
                return;

            ApplySignColors(panel, reputation);
        }
    }
}
