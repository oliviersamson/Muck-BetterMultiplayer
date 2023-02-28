using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace BetterMultiplayer.GameoverUIPatch
{
    static class PrefixesAndPostfixes
    {
        private static IEnumerator LoadLobby(Lobby currentLobby)
        {          
            var asyncLoadLevel = SceneManager.LoadSceneAsync("Menu");
            while (!asyncLoadLevel.isDone)
            {
                Debug.Log("Loading Lobby");
                yield return null;
            }

            Plugin.Log.LogDebug("Joining Lobby");
            LobbyVisuals.Instance.OpenLobby(currentLobby);
            LoadingScreen.Instance.CancelInvoke("CheckAllPlayersLoading");
        }

        [HarmonyPatch(typeof(GameoverUI), "Awake")]
        [HarmonyPostfix]
        static void PatchStart(GameoverUI __instance)
        {
            NetworkController.Instance.loading = false;

            GameObject header = GameObject.Find("Header");
            GameObject menuButton = GameObject.Find("MenuButton");

            Button returnToLobbyButton = UnityEngine.Object.Instantiate(menuButton, header.transform).GetComponent<Button>();
            returnToLobbyButton.name = "LobbyButton";
            returnToLobbyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lobby";

            RectTransform rectTransform = returnToLobbyButton.GetComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 25f, rectTransform.sizeDelta.x + 20f);

            for (int i = 0; i < returnToLobbyButton.onClick.GetPersistentEventCount(); i++)
            {
                returnToLobbyButton.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);
            }

            GameObject scroll = GameObject.Find("Scroll");

            RectTransform scrollRectTransform = scroll.GetComponent<RectTransform>();
            scrollRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 200f, scrollRectTransform.sizeDelta.x);

            RectTransform textRectTransform = __instance.nameText.GetComponent<RectTransform>();
            textRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 165f, textRectTransform.sizeDelta.x);

            returnToLobbyButton.onClick.AddListener(
                () => {
                    Plugin.Log.LogDebug("Returning to Lobby");

                    if (LocalClient.serverOwner)
                    {
                        Debug.LogError("Host left game");
                        //AccessTools.Method(typeof(GameManager), "HostLeftGame").Invoke(GameManager.instance, null);
                    }
                    else
                    {
                        //ClientSend.PlayerDisconnect();
                    }

                    foreach (var clients in Server.clients)
                    {
                        if (clients.Value.player != null)
                        {
                            clients.Value.StartClientSteam(clients.Value.player.username, new UnityEngine.Color(), clients.Value.player.steamId);
                        }                       
                    }

                    SteamManager.Instance.currentLobby.SetPublic();
                    SteamManager.Instance.currentLobby.SetJoinable(true);

                    AccessTools.Field(typeof(SteamLobby), "started").SetValue(SteamLobby.Instance, false);
                    AccessTools.Field(typeof(SteamLobby), "currentLobby").SetValue(SteamLobby.Instance, SteamManager.Instance.currentLobby);

                    SteamLobby.Instance.StartCoroutine(LoadLobby(SteamManager.Instance.currentLobby));
                });
        }
    }
}
