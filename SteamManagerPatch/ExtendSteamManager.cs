using BetterMultiplayer;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public static class SteamManager_Method_Extensions
    {
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
    }
}
