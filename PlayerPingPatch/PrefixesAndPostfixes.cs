using HarmonyLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
