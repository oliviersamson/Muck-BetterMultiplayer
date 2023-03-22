using HarmonyLib;

namespace BetterMultiplayer.ChestInteractPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(ChestInteract), "GetName")]
        [HarmonyPrefix]
        static bool PatchIsUsed(ref string __result, ChestInteract __instance)
        {
            __result = string.Format("{0}\n<size=50%>(Press \"{1}\" to open", __instance.state.ToString(), InputManager.interact);

            return false;
        }
    }
}
