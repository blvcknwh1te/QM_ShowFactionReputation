using System.Reflection;
using HarmonyLib;
using MGSC;
using UnityEngine;

namespace ReputationOnMissionTooltip
{
    public static class Plugin
    {
        public const string HarmonyId = "blvcknwh1te.QM_ReputationOnMissionTooltip";

        public static string ModAssemblyName => Assembly.GetExecutingAssembly().GetName().Name;

        [Hook(ModHookType.BeforeBootstrap)]
        public static void Bootstrap(IModContext context)
        {
            new Harmony(HarmonyId).PatchAll();
            Debug.Log($"[{ModAssemblyName}] Harmony patches applied.");
        }
    }
}
