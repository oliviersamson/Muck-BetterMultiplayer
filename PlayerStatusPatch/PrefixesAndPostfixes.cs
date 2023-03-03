using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterMultiplayer.PlayerStatusPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PlayerStatus), "PlayerDied")]
        [HarmonyPostfix]
        static void PlayerDiedPostfix()
        {
            Map.Instance.PlayerDied(GameManager.players[LocalClient.instance.myId].username);
        }
    }
}
