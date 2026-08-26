using HarmonyLib;
using MGSC;

namespace ShowFactionReputation
{
    // Первая строка блока миссии (до Миссия/Сложность/…).
    [HarmonyPatch(typeof(TooltipFactory), "AddMissionDescription")]
    internal static class TooltipFactory_AddMissionDescription_Patch
    {
        private static readonly AccessTools.FieldRef<TooltipFactory, State> StateField =
            AccessTools.FieldRefAccess<TooltipFactory, State>("_state");

        public static void Prefix(TooltipFactory __instance, Mission mission)
        {
            if (__instance == null || mission == null)
                return;

            State state = StateField(__instance);
            Factions factions = state?.Get<Factions>();
            Faction faction = factions?.Get(mission.BeneficiaryFactionId);
            if (faction == null)
                return;

            ReputationTooltip.AddMissionRow(__instance, faction.PlayerReputation);
        }
    }
}
