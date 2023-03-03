using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public static class ExtendPlayerPing
    {
        private static readonly ConditionalWeakTable<PlayerPing, MapMarkerObject> MapMarkers = new ConditionalWeakTable<PlayerPing, MapMarkerObject>();

        public class MapMarkerObject
        {
            public Map.MapMarker Value { get; set; }
        }

        public static MapMarkerObject MapMarker(this PlayerPing playerPing)
        {
            return MapMarkers.GetOrCreateValue(playerPing);
        }
    }
}
