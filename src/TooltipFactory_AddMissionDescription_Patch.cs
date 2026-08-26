using HarmonyLib;
using MGSC;

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

            // Подпись из локализации игры (tooltip.reputation) — любой язык клиента.
            float rep = faction.PlayerReputation;
            TooltipProperty panel = __instance.AddPanelToTooltip();
            panel.SetIcon("common_reputation")
                .LocalizeName("tooltip.reputation")
                .SetValue(FormatHelper.ToInt(rep, showPlus: true));

            // 0 — дефолт панели; минус/плюс — ванильные LightRed / AltGreen (мягкий плюс).
            if (rep < 0f)
            {
                panel.SetValueColor(Colors.LightRed);
                panel.SetNameColor(Colors.LightRed);
            }
            else if (rep > 0f)
            {
                panel.SetValueColor(Colors.AltGreen);
                panel.SetNameColor(Colors.AltGreen);
            }
        }
    }
}
