using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace BetterMultiplayer.LobbySettingsPatch
{
    static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(LobbySettings), "Start")]
        [HarmonyPrefix]
        static void PatchIsUsed()
        {
            Plugin.Log.LogDebug($"Resetting Buttons Dictionary.");

            LobbyData.Buttons = new()
            {
                { "Setting_Difficulty", new List<Button>() },
                { "Setting_PlayerDamage", new List<Button>() },
                { "Setting_Gamemdoe", new List<Button>() } // Not a typo, this is its name!
            };
        }
    }
}
