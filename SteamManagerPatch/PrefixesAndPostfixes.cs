using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
