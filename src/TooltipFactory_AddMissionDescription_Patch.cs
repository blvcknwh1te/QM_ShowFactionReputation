using HarmonyLib;
using MGSC;
using UnityEngine;

namespace ShowFactionReputation
{
    // Первая строка блока миссии (до Миссия/Сложность/…) — как ванильные панели.
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
            if (state == null)
                return;

            Factions factions = state.Get<Factions>();
            Faction faction = factions?.Get(mission.BeneficiaryFactionId);
            if (faction == null)
                return;

            // Подпись из локализации игры (tooltip.Reputation) — любой язык клиента.
            TooltipProperty panel = __instance.AddPanelToTooltip();
            panel.SetIcon("common_reputation")
                .LocalizeName("tooltip.Reputation")
                .SetValue(FormatHelper.ToInt(faction.PlayerReputation, showPlus: true));

            if (faction.PlayerReputation < 0f)
            {
                panel.SetValueColor(Colors.LightRed);
                panel.SetNameColor(Colors.LightRed);
            }
        }
    }
}
