using HarmonyLib;
using Steamworks.Data;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BetterMultiplayer.LobbyVisualsPatch
{  
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(LobbyVisuals), "OpenLobby")]
        [HarmonyPostfix]
        static void PatchOpenLobby(Lobby lobby)
        {
            // Reinitialize LobbyData
            LobbyData.LobbyBuffer = lobby;

            bool isLobbyOwner = SteamManager.Instance.PlayerSteamId.Value == lobby.Owner.Id;

            LobbySettings.Instance.seed.enabled = isLobbyOwner;

            if (!isLobbyOwner) 
            {
                Plugin.Log.LogDebug("Lobby input fields control set to lobby owner.");
                return;
            }

            foreach (KeyValuePair<string, List<Button>> settingsButtons in LobbyData.Buttons)
            {
                UiSettings uiSettings = null;

                if (settingsButtons.Key == "Setting_Difficulty")
                {
                    uiSettings = LobbySettings.Instance.difficultySetting;   
                }
                else if (settingsButtons.Key == "Setting_PlayerDamage")
                {
                    uiSettings = LobbySettings.Instance.friendlyFireSetting;
                }
                else if (settingsButtons.Key == "Setting_Gamemdoe")
                {
                    uiSettings = LobbySettings.Instance.gamemodeSetting;
                }

                int setting = 0;
                foreach (Button button in settingsButtons.Value)
                {
                    int currentSetting = setting;

                    button.onClick.AddListener(delegate ()
                    {
                        AccessTools.Method(typeof(UiSettings), "UpdateSetting", new Type[] { typeof(int) }).Invoke(uiSettings, new object[] { currentSetting });
                    });

                    setting++;
                }
            }

            Plugin.Log.LogDebug("Taken control of lobby input fields.");
        }
    }
}
