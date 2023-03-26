using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace BetterMultiplayer.SteamManagerPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(SteamManager), "Start")]
        [HarmonyPrefix]
        static void PatchStart(SteamManager __instance)
        {
            SteamMatchmaking.OnLobbyCreated += __instance.SetLobbySettings;
            SteamMatchmaking.OnLobbyEntered += __instance.UpdateLobbySettings;
            SteamMatchmaking.OnLobbyDataChanged += __instance.UpdateLobbySettings;
            SteamMatchmaking.OnLobbyMemberDataChanged += __instance.UpdateMemberGameState;
        }
    }
}
