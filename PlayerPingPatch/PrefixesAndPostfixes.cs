using HarmonyLib;
using UnityEngine;

namespace BetterMultiplayer.PlayerPingPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PlayerPing), "HidePing")]
        [HarmonyPostfix]
        static void HidePingPostfix(PlayerPing __instance)
        {
            Map.Instance.RemoveMarker(__instance.MapMarker().Value);
        }
    }
}
