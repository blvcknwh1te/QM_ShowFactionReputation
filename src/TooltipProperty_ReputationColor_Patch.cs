using HarmonyLib;
using MGSC;

namespace ShowFactionReputation
{
    // Ванильные строки репутации (станция с наценкой и др.) — тот же SSOT цвета.
    [HarmonyPatch(typeof(TooltipProperty), nameof(TooltipProperty.LocalizeName))]
    internal static class TooltipProperty_LocalizeName_Patch
    {
        public static void Postfix(TooltipProperty __instance, string tag)
        {
            ReputationTooltip.NoteLocalizedName(__instance, tag);
        }
    }

    [HarmonyPatch(typeof(TooltipProperty), nameof(TooltipProperty.SetValue), typeof(string), typeof(bool))]
    internal static class TooltipProperty_SetValue_Patch
    {
        public static void Postfix(TooltipProperty __instance, string val)
        {
            ReputationTooltip.ApplyColorsIfPending(__instance, val);
        }
    }
}
