using HarmonyLib;
using MGSC;
using TMPro;

namespace ShowFactionReputation
{
    [HarmonyPatch(typeof(TooltipFactionHeader), nameof(TooltipFactionHeader.Initialize))]
    [HarmonyPriority(Priority.Last)]
    internal static class TooltipFactionHeader_Initialize_Patch
    {
        // Reflection: без IgnoresAccessChecksTo прямой доступ к private-полю валит Postfix
        // и обрывает TooltipFactory.BuildStationTooltip до списка предметов станции.
        private static readonly AccessTools.FieldRef<TooltipFactionHeader, TextMeshProUGUI> TechLevelDesc =
            AccessTools.FieldRefAccess<TooltipFactionHeader, TextMeshProUGUI>("_techLevelDesc");

        // Last: после soft-соседей вроде ShowTechLevel, дописываем к уже собранному тексту ТУ
        public static void Postfix(TooltipFactionHeader __instance, Faction faction)
        {
            if (__instance == null || faction == null)
                return;

            TextMeshProUGUI techLevelDesc = TechLevelDesc(__instance);
            if (techLevelDesc == null)
                return;

            string current = techLevelDesc.text;
            techLevelDesc.SetText(
                ReputationFormat.AppendToTechLevel(current, faction.PlayerReputation));
        }
    }
}
