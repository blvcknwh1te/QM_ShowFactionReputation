using HarmonyLib;
using MGSC;

namespace ShowFactionReputation
{
    // Ваниль (станция + наценка): красим ведущее число в SetValue до записи в TMP.
    [HarmonyPatch(typeof(TooltipProperty), nameof(TooltipProperty.SetValue), typeof(string), typeof(bool))]
    internal static class TooltipProperty_SetValue_Patch
    {
        public static void Prefix(TooltipProperty __instance, ref string val, ref bool firstLetterToUpperCase)
        {
            ReputationTooltip.PrepareValueForReputationRow(__instance, ref val, ref firstLetterToUpperCase);
        }
    }
}
