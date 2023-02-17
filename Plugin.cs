using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace MultiAccess
{
    // - OtherInput.ToggleInventory is responsible of copying chest inventory when opened. Basically creating another copy of the chest to view
    // - ChestManager.SendChestUpdate will lock the chest cell that was changed (only for this particular user) when sending chest update data to server after this particular change
    //   ChestManager.UpdateChest will unlock the chest cell (only for this particular user) when data is received from packets about an updated chest
    //   This probably blocks the user to change the cell again before receiving data back from the server and client chest data won't be updated before receiving this data
    //   Deactivating a GameObject disables each component, including attached renderers, colliders, rigidbodies, and scripts. For example,
    //   Unity will no longer call the Update() method of a script attached to a deactivated GameObject. OnEnable or OnDisable are called as the GameObject received SetActive(true) or SetActive(false).

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Muck.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            //Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log = base.Logger;

            harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(ChestPatch));

            Log.LogInfo($"Mod {PluginInfo.PLUGIN_NAME} loaded!");
        }
    }

    class ChestPatch
    {
        private static int tempChestId = 0;

        [HarmonyPatch(typeof(Chest), "IsUsed")]
        [HarmonyPrefix]
        static bool PatchIsUsed(ref bool __result)
        {
            
            __result = false;

            Plugin.Log.LogInfo($"Chest.IsUsed overwriten");

            return false;
        }
    }

    [HarmonyPatch(typeof(ClientHandle), "UpdateChest")]
    class ClientHandle_UpdateChest_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            // Match end of method
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ret));



            return codeMatcher.InstructionEnumeration();
        }
    }
}
