using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterMultiplayer
{
    public static class Globals
    {
        public const string PLUGIN_GUID = "muck.mrboxxy.bettermultiplayer";
        public const string PLUGIN_NAME = "BetterMultiplayer";
        public const string PLUGIN_VERSION = "0.1.0";
    }

    [BepInPlugin(Globals.PLUGIN_GUID, Globals.PLUGIN_NAME, Globals.PLUGIN_VERSION)]
    [BepInProcess("Muck.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            harmony = new Harmony(Globals.PLUGIN_NAME);

            harmony.PatchAll(typeof(ChestPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched Chest.IsUsed()");

            harmony.PatchAll(typeof(ClientHandlePatch.UpdateChestTranspiler));
            Log.LogInfo("Patched ClientHandle.UpdateChest(Packet)");

            harmony.PatchAll(typeof(ServerHandlePatch.UpdateChestTranspiler));
            Log.LogInfo("Patched ServerHandle.UpdateChest(int, Packet)");
        }
    }



    

    
}
