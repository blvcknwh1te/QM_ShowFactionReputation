using System.Reflection;
using HarmonyLib;
using MGSC;
using UnityEngine;

namespace ShowFactionReputation
{
    public static class Plugin
    {
        public const string HarmonyId = "blvcknwh1te.QM_ShowFactionReputation";

        public static string ModAssemblyName => Assembly.GetExecutingAssembly().GetName().Name;

        [Hook(ModHookType.BeforeBootstrap)]
        public static void Bootstrap(IModContext context)
        {
            new Harmony(HarmonyId).PatchAll();
            Debug.Log($"[{ModAssemblyName}] Harmony patches applied.");
        }
    }
}
