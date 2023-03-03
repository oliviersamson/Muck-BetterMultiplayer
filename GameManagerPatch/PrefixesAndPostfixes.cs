using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BetterMultiplayer.GameManagerPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(GameManager), "RespawnPlayer")]
        [HarmonyPostfix]
        static void RespawnPlayerPostfix(int id)
        {
            if (GameManager.players.ContainsKey(id))
            {
                Map.Instance.PlayerRevived(GameManager.players[id].username);
            } 
        }

        [HarmonyPatch(typeof(GameManager), "KillPlayer")]
        [HarmonyPostfix]
        static void KillPlayerPostfix(int id)
        {
            if (GameManager.players.ContainsKey(id))
            {
                Map.Instance.PlayerDied(GameManager.players[id].username);
            }           
        }
    }
}
