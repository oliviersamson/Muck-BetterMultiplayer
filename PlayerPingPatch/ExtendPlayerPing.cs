using System.Runtime.CompilerServices;

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
