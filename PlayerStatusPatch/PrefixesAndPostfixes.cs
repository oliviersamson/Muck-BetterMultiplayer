using HarmonyLib;
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
