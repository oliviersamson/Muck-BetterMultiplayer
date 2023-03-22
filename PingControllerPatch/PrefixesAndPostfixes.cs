using HarmonyLib;
using UnityEngine;

namespace BetterMultiplayer.PingControllerPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PingController), "MakePing")]
        [HarmonyPrefix]
        static bool MakePingPrefix(PingController __instance, Vector3 pos, string name, string pingedName)
        {
            // Do as original, but keep the GameObject and PlayerPing to use to add map marker
            GameObject gameObject = GameObject.Instantiate<GameObject>(__instance.pingPrefab, pos, Quaternion.identity);
            PlayerPing playerPing = gameObject.GetComponent<PlayerPing>();
            playerPing.SetPing(name, pingedName);

            // Add map marker
            playerPing.MapMarker().Value = Map.Instance.AddMarker(gameObject.transform, Map.MarkerType.Other, null, new Color(0f, 0.8105783f, 1f), "<size=50%>" + name);

            // Overwrite original method
            return false;
        }
    }
}
