using System;
using System.Globalization;
using MGSC;
using UnityEngine;

namespace ShowFactionReputation
{
    /// <summary>SSOT: подпись, значение и цвет строки репутации в тултипах.</summary>
    internal static class ReputationTooltip
    {
        public const string LocKey = "tooltip.reputation";
        public const string IconTag = "common_reputation";

        private static readonly string[] LabelKeys =
        {
            LocKey,
            "ui.tradeshuttle.mode.reputation",
        };

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

        public static Color SignColor(float reputation)
        {
            if (reputation < 0f)
                return Colors.LightRed;
            if (reputation > 0f)
                return Colors.Green;
            return Colors.White;
        }

        public static void ApplyNameColor(TooltipProperty panel, float reputation)
        {
            if (panel == null || reputation == 0f)
                return;

            panel.SetNameColor(SignColor(reputation));
        }

        /// <summary>Число (+/-) в &lt;color&gt;; хвост (наценка и т.п.) не трогаем.</summary>
        public static string ColorizeLeadingNumber(string value, float reputation)
        {
            if (string.IsNullOrEmpty(value) || reputation == 0f)
                return value;

            if (!TryGetLeadingNumberLength(value, out int length))
                return value;

            string hex = "#" + ColorUtility.ToHtmlStringRGB(SignColor(reputation));
            return "<color=" + hex + ">" + value.Substring(0, length) + "</color>" + value.Substring(length);
        }

        public static string FormatMissionValue(float reputation)
        {
            string number = FormatHelper.ToInt(reputation, showPlus: true);
            return ColorizeLeadingNumber(number, reputation);
        }

        public static bool TryParseLeadingReputation(string value, out float reputation)
        {
            reputation = 0f;
            if (!TryGetLeadingNumberLength(value, out int length))
                return false;

            return float.TryParse(
                value.Substring(0, length),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out reputation);
        }

        public static bool IsReputationRow(TooltipProperty panel)
        {
            if (panel == null || panel.Name == null)
                return false;

            string current = panel.Name.text;
            if (string.IsNullOrEmpty(current))
                return false;

            // SetName всегда делает FirstLetterToUpperCase от результата Get.
            foreach (string key in LabelKeys)
            {
                string loc = Localization.Get(key, warnIfMissingTag: false);
                if (string.IsNullOrEmpty(loc) || loc == key)
                    continue;

                if (string.Equals(current, loc, StringComparison.Ordinal))
                    return true;
                if (string.Equals(current, FormatHelper.FirstLetterToUpperCase(loc), StringComparison.Ordinal))
                    return true;
            }

            // Старый артефакт промаха локализации.
            return string.Equals(current, "Tooltip.reputation", StringComparison.Ordinal);
        }

        public static void AddMissionRow(TooltipFactory factory, float reputation)
        {
            TooltipProperty panel = factory.AddPanelToTooltip();
            panel.SetIcon(IconTag);
            panel.SetName(LocalizedLabel());
            // false: не ломать <color> у числа
            panel.SetValue(FormatMissionValue(reputation), firstLetterToUpperCase: false);
            ApplyNameColor(panel, reputation);
        }

        /// <summary>Prefix SetValue: станция с наценкой и любые LocalizeName(tooltip.reputation).</summary>
        public static void PrepareValueForReputationRow(
            TooltipProperty panel,
            ref string value,
            ref bool firstLetterToUpperCase)
        {
            if (!IsReputationRow(panel))
                return;

            // Иначе FirstLetterToUpperCase портит теги и наш parse.
            firstLetterToUpperCase = false;

            if (!TryParseLeadingReputation(value, out float reputation))
                return;

            value = ColorizeLeadingNumber(value, reputation);
            ApplyNameColor(panel, reputation);
        }

        private static bool TryGetLeadingNumberLength(string value, out int length)
        {
            length = 0;
            if (string.IsNullOrEmpty(value))
                return false;

            // Уже обёрнуто нами — не трогаем повторно.
            if (value.StartsWith("<color=", StringComparison.Ordinal))
                return false;

            int i = 0;
            if (value[0] == '+' || value[0] == '-')
                i = 1;

            int start = i;
            while (i < value.Length && char.IsDigit(value[i]))
                i++;

            if (i == start)
                return false;

            length = i;
            return true;
        }
    }
}
