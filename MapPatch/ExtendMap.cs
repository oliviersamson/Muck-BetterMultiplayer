using BetterMultiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    public static class ExtendMap
    {
        public static void PlayerDied(this Map map, string name)
        {
            Map.MapMarker player = map.mapMarkers.Find(
                (m) => {
                    if (name == m.marker.GetComponentInChildren<TextMeshProUGUI>().text)
                        return true;
                    return false;
                });

            if (player != default)
            {
                player.marker.GetComponent<RawImage>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
        public static void PlayerRevived(this Map map, string name)
        {
            Map.MapMarker player = map.mapMarkers.Find(
                (m) => {
                    if (name == m.marker.GetComponentInChildren<TextMeshProUGUI>().text)
                        return true;
                    return false;
                });

            if (player != default)
            {
                player.marker.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
