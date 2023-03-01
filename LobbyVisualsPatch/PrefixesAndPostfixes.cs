using HarmonyLib;
using Steamworks.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        [HarmonyPatch(typeof(LobbyVisuals), "Awake")]
        [HarmonyPostfix]
        static void AwakePostfix()
        {
            GameObject lobbyID = GameObject.Find("LobbyID");
            RectTransform rectTransform = lobbyID.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 60f);

            GameObject menuButton = GameObject.Find("MenuButton");
            Button inviteFriend = UnityEngine.Object.Instantiate(menuButton.transform.GetParent().gameObject, lobbyID.transform).GetComponentInChildren<Button>();
            inviteFriend.name = "InviteFriend";
            inviteFriend.GetComponentInChildren<TextMeshProUGUI>().text = "Invite friend(s)";

            for (int i = 0; i < inviteFriend.onClick.GetPersistentEventCount(); i++)
            {
                inviteFriend.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);
            }

            inviteFriend.onClick.AddListener(
                () => {
                    Plugin.Log.LogDebug("Inviting friends through steam overlay");

                    SteamManager.Instance.OpenFriendOverlayForGameInvite();
                });
        }
    }
}
