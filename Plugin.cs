using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterMultiplayer
{
    public static class Globals
    {
        public const string PLUGIN_GUID = "muck.mrboxxy.bettermultiplayer";
        public const string PLUGIN_NAME = "BetterMultiplayer";
        public const string PLUGIN_VERSION = "0.5.0";
    }

    [BepInPlugin(Globals.PLUGIN_GUID, Globals.PLUGIN_NAME, Globals.PLUGIN_VERSION)]
    [BepInProcess("Muck.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public static AssetBundle Bundle;

        public static Transform Overlay;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            Bundle = GetAssetBundle("uiassets");
            Overlay = Bundle.LoadAsset<GameObject>("ReturningLobby").GetComponent<Transform>();
            Bundle.Unload(false);

            Overlay.GetChild(0).GetChild(0).GetChild(1).gameObject.AddComponent<ButtonSfx>();

            harmony = new Harmony(Globals.PLUGIN_NAME);

            harmony.PatchAll(typeof(ChestPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched Chest.IsUsed()");

            harmony.PatchAll(typeof(ClientHandlePatch.UpdateChestTranspiler));
            harmony.PatchAll(typeof(ClientHandlePatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched ClientHandle.UpdateChest(Packet)");
            Log.LogInfo("Patched ClientHandle.OpenChest(Packet)");

            harmony.PatchAll(typeof(ServerHandlePatch.UpdateChestTranspiler));
            harmony.PatchAll(typeof(ServerHandlePatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched ServerHandle.UpdateChest(int, Packet)");       
            Log.LogInfo("Patched ServerHandle.RequestChest(int, Packet)");

            harmony.PatchAll(typeof(ChestInteractPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched ChestInteract.GetName()");

            harmony.PatchAll(typeof(LobbySettingsPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched LobbySettings.Start()");

            harmony.PatchAll(typeof(UiSettingsPatch.AddSettingsTranspiler));
            Log.LogInfo("Patched UiSettings.AddSettings(int, string[])");

            harmony.PatchAll(typeof(LobbyVisualsPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched LobbyVisuals.OpenLobby(Lobby)");

            harmony.PatchAll(typeof(SteamManagerPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched SteamManager.Start()");

            harmony.PatchAll(typeof(GameoverUIPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched GameoverUI.Awake()");

            harmony.PatchAll(typeof(MapPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched Map.UpdateMap()");

            harmony.PatchAll(typeof(GameManagerPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched GameManager.KillPlayer()");
            Log.LogInfo("Patched GameManager.RevivePlayer()");
            Log.LogInfo("Patched GameManager.Start()");

            harmony.PatchAll(typeof(PlayerStatusPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched PlayerStatus.PlayerDied()");

            harmony.PatchAll(typeof(PingControllerPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched PingController.MakePing()");

            harmony.PatchAll(typeof(PlayerPingPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched PlayerPing.HidePing()");

            harmony.PatchAll(typeof(UiCControllerPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched UiCController.Awake()");
        }

        public static AssetBundle GetAssetBundle(string name)
        {
            var execAssembly = Assembly.GetExecutingAssembly();

            var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

            using (var stream = execAssembly.GetManifestResourceStream(resourceName))
            {
                return AssetBundle.LoadFromStream(stream);
            }
        }
    }
}
