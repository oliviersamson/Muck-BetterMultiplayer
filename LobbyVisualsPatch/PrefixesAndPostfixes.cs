using HarmonyLib;
using Steamworks.Data;
using System.Collections.Generic;
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
            bool isLobbyOwner = SteamManager.Instance.PlayerSteamId.Value == lobby.Owner.Id;

            isLobbyOwner = false;

            LobbySettings.Instance.seed.interactable = isLobbyOwner;

            foreach (KeyValuePair<string, UiSettings> uiSettings in LobbySettings.Instance.GetSettings())
            {
                foreach (Button button in uiSettings.Value.GetButtons())
                {
                    button.interactable = isLobbyOwner;
                }
            }

            if (isLobbyOwner)
            {
                Plugin.Log.LogDebug("Taken control of lobby input fields.");
            }
            else
            {
                Plugin.Log.LogDebug("Lobby input fields control set to lobby owner.");
            }
        }
    }
}
