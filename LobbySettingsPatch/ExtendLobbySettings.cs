using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
