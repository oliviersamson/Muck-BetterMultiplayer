using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
    public static class ExtendLobbySettings
    {
        private static readonly ConditionalWeakTable<LobbySettings, Dictionary<string, UiSettings>> Buttons = new ConditionalWeakTable<LobbySettings, Dictionary<string, UiSettings>>();

        public static Dictionary<string, UiSettings> GetSettings(this LobbySettings lobbySettings)
        {
            return Buttons.GetOrCreateValue(lobbySettings);
        }
    }
}
