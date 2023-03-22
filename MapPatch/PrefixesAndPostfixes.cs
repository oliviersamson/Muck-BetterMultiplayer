using HarmonyLib;
using System;
using UnityEngine;

namespace BetterMultiplayer.MapPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(Map), "UpdateMap")]
        [HarmonyPrefix]
        static bool UpdateMapPrefix(Map __instance)
        {
            foreach (Map.MapMarker mapMarker in __instance.mapMarkers)
            {
                if (mapMarker != null)
                {
                    if (mapMarker.worldObject == null || (mapMarker.type != Map.MarkerType.Player && !mapMarker.worldObject.gameObject.activeInHierarchy))
                    {
                        mapMarker.marker.gameObject.SetActive(false);
                    }
                    else
                    {
                        mapMarker.marker.gameObject.SetActive(true);
                        mapMarker.marker.localPosition = (Vector3)AccessTools.Method(typeof(Map), "WorldPositionToMap", new Type[] { typeof(Vector3) }).Invoke(__instance, new object[] { mapMarker.worldObject.position });
                    }
                }                
            }

            return false;
        }
    }
}
