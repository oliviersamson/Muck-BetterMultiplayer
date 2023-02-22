using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace BetterMultiplayer.LobbySettingsPatch
{
    static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(LobbySettings), "Start")]
        [HarmonyPrefix]
        static void PatchStart(LobbySettings __instance)
        {
            Plugin.Log.LogDebug($"Adding Muck's original UiSettings to LobbySettings' settings list.");

            __instance.GetSettings().Add(__instance.difficultySetting.name, __instance.difficultySetting);
            __instance.GetSettings().Add(__instance.friendlyFireSetting.name, __instance.friendlyFireSetting);
            __instance.GetSettings().Add(__instance.gamemodeSetting.name, __instance.gamemodeSetting);
        }
    }
}
