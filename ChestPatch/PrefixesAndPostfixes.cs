using HarmonyLib;

namespace BetterMultiplayer.ChestPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(Chest), "IsUsed")]
        [HarmonyPrefix]
        static bool PatchIsUsed(ref bool __result)
        {
            __result = false;

            Plugin.Log.LogInfo($"Chest.IsUsed overwriten");

            return false;
        }
    }
}
