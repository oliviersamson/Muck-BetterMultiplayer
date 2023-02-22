using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace UnityEngine
{
    public static class ExtendUiSettings
    {
        private static readonly ConditionalWeakTable<UiSettings, List<Button>> Buttons = new ConditionalWeakTable<UiSettings, List<Button>>();

        public static List<Button> GetButtons(this UiSettings uiSettings)
        {
            return Buttons.GetOrCreateValue(uiSettings);
        }
    }
}
