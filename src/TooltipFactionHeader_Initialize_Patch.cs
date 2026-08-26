using HarmonyLib;
using MGSC;
using TMPro;

namespace ShowFactionReputation
{
    [HarmonyPatch(typeof(TooltipFactionHeader), nameof(TooltipFactionHeader.Initialize))]
    [HarmonyPriority(Priority.Last)]
    internal static class TooltipFactionHeader_Initialize_Patch
    {
        // Last: после soft-соседей вроде ShowTechLevel, дописываем к уже собранному тексту ТУ
        public static void Postfix(TooltipFactionHeader __instance, Faction faction)
        {
            if (__instance == null || faction == null || __instance._techLevelDesc == null)
                return;

            string current = __instance._techLevelDesc.text;
            __instance._techLevelDesc.SetText(
                ReputationFormat.AppendToTechLevel(current, faction.PlayerReputation));
        }
    }
}
