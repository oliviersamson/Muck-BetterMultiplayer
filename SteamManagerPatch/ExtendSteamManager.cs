using BetterMultiplayer;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace UnityEngine
{
    public static class ExtendSteamManager
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

        public static void ReturnToLobby(this SteamManager steamManager) 
        {
            Plugin.Log.LogDebug("Returning to Lobby");

            foreach (var clients in Server.clients)
            {
                if (clients.Value.player != null)
                {
                    clients.Value.StartClientSteam(clients.Value.player.username, new UnityEngine.Color(), clients.Value.player.steamId);
                }
            }

            steamManager.currentLobby.SetPublic();
            steamManager.currentLobby.SetJoinable(true);

            AccessTools.Field(typeof(SteamLobby), "started").SetValue(SteamLobby.Instance, false);
            AccessTools.Field(typeof(SteamLobby), "currentLobby").SetValue(SteamLobby.Instance, steamManager.currentLobby);

            SteamLobby.Instance.StartCoroutine(LoadLobby(SteamManager.Instance.currentLobby));
        }

        public static void UpdateLobbySettings(this SteamManager steamManager, Lobby lobby)
        {
            foreach (KeyValuePair<string, UiSettings> uiSettings in LobbySettings.Instance.GetSettings())
            {
                int settingValue = Int32.Parse(lobby.GetData(uiSettings.Key));
                AccessTools.Method(typeof(UiSettings), "UpdateSetting", new Type[] { typeof(int) }).Invoke(uiSettings.Value, new object[] { settingValue });
            }

            LobbySettings.Instance.seed.text = lobby.GetData("seed");
        }

        public static void SetLobbySettings(this SteamManager steamManager, Result result, Lobby lobby)
        {
            if (result != Result.OK) return;

            foreach (KeyValuePair<string, UiSettings> uiSettings in LobbySettings.Instance.GetSettings())
            {
                lobby.SetData(uiSettings.Key, uiSettings.Value.setting.ToString());
            }

            lobby.SetData("seed", LobbySettings.Instance.seed.text);
        }

        public static void SetLobbySetting(this SteamManager steamManager, string setting, int settingValue)
        {
            steamManager.currentLobby.SetData(setting, settingValue.ToString());

            Plugin.Log.LogDebug($"Updating lobby setting {setting} with value {settingValue}");
        }

        public static void SetLobbySeed(this SteamManager steamManager, string seed)
        {
            steamManager.currentLobby.SetData("seed", seed);
        }

        public static void UpdateMemberGameState(this SteamManager steamManager, Lobby lobby, Friend friend)
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if (friend.Id == lobby.Owner.Id && lobby.GetMemberData(friend, "game_state") == "lobby")
                {
                    steamManager.ReturnToLobby();
                }
            }
        }

        public static void SetGameStateToLobby(this SteamManager steamManager)
        {
            SteamManager.Instance.currentLobby.SetMemberData("game_state", "lobby");
        }

        public static IEnumerator LobbyCountdown(this SteamManager steamManager)
        {
            string msg = "Going back to lobby in 3...";
            ClientSend.SendChatMessage(msg);
            ChatBox.Instance.AppendMessage(-1, msg, "");
            yield return new WaitForSeconds(1f);

            msg = "Going back to lobby in 2...";
            ClientSend.SendChatMessage(msg);
            ChatBox.Instance.AppendMessage(-1, msg, "");
            yield return new WaitForSeconds(1f);

            msg = "Going back to lobby in 1...";
            ClientSend.SendChatMessage(msg);
            ChatBox.Instance.AppendMessage(-1, msg, "");
            yield return new WaitForSeconds(1f);

            steamManager.SetGameStateToLobby();
        }
    }
}
