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
        static void PatchPreStart(LobbySettings __instance)
        {
            Plugin.Log.LogDebug($"Adding Muck's original UiSettings to LobbySettings' settings list.");

            __instance.GetSettings().Add(__instance.difficultySetting.name, __instance.difficultySetting);
            __instance.GetSettings().Add(__instance.friendlyFireSetting.name, __instance.friendlyFireSetting);
            __instance.GetSettings().Add(__instance.gamemodeSetting.name, __instance.gamemodeSetting);
        }

        [HarmonyPatch(typeof(LobbySettings), "Start")]
        [HarmonyPostfix]
        static void PatchPostStart(LobbySettings __instance)
        {
            Plugin.Log.LogDebug($"Setting callbacks to update lobby settings data through the buttons.");

            foreach(KeyValuePair<string, UiSettings> uiSettings in __instance.GetSettings())
            {
                for (int i = 0; i < uiSettings.Value.GetButtons().Count; i++)
                {
                    string settingName = uiSettings.Key;
                    int settingValue = i;

                    uiSettings.Value.GetButtons()[i].onClick.AddListener(delegate ()
                    {
                        SteamManager.Instance.SetLobbySetting(settingName, settingValue);
                    });
                }
            }

            __instance.seed.onValueChanged.AddListener(delegate (string newSeed)
            {
                SteamManager.Instance.SetLobbySeed(newSeed);
            });
        }
    }
}
