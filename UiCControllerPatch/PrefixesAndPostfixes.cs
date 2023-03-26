using HarmonyLib;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BetterMultiplayer.UiCControllerPatch
{
    public static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(UiController), "Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(UiController __instance)
        {
            Plugin.Log.LogDebug("Adding new Lobby button");
            Transform pauseUi = __instance.transform.Find("PauseUI");
            GameObject leaveBtn = pauseUi.Find("LeaveBtn").gameObject;

            GameObject lobbyBtn = GameObject.Instantiate(leaveBtn, pauseUi);
            lobbyBtn.name = "LobbyBtn";
            lobbyBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Lobby";

            leaveBtn.transform.SetAsLastSibling();

            Button button = lobbyBtn.GetComponent<Button>();

            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                button.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);
            }

            if (SteamManager.Instance.currentLobby.IsOwnedBy(SteamManager.Instance.PlayerSteamId))
            {
                button.onClick.AddListener(
                    () =>
                    {

                        if (GameManager.instance.GetPlayersInLobby() == 1)
                        {
                            SteamManager.Instance.SetGameStateToLobby();
                            return;
                        }

                        button.interactable = false;

                        OtherInput.Instance.Unpause();

                        IEnumerator coroutine = SteamManager.Instance.LobbyCountdown();

                        SteamManager.Instance.StartCoroutine(coroutine);
                    });
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}
