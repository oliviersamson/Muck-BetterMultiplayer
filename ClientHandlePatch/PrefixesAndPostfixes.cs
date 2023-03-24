using HarmonyLib;
using System;
using UnityEngine;

namespace BetterMultiplayer.ClientHandlePatch
{
    static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(ClientHandle), "OpenChest")]
        [HarmonyPrefix]
        static bool OpenChestPrefix(Packet packet)
        {
            int num = packet.ReadInt(true);
            int num2 = packet.ReadInt(true);
            bool flag = packet.ReadBool(true);
            bool stillInUse = false;

            try {
                stillInUse = packet.ReadBool(true);
            }
            catch (Exception e) {
                Plugin.Log.LogInfo("Can't read next bool data. It seems the host doesn't have the latest version of BetterMultiplayer installed.");
            }

            MonoBehaviour.print(string.Format("player{0} now {1} chest{2}", num, flag, num2));

            ChestManager.Instance.UseChest(num2, stillInUse || flag);

            if (flag && num == LocalClient.instance.myId)
            {
                if (OtherInput.Instance.currentChest != null)
                {
                    ClientSend.RequestChest(OtherInput.Instance.currentChest.id, false);
                    OtherInput.Instance.currentChest = null;
                }
                OtherInput.Instance.currentChest = ChestManager.Instance.chests[num2];
                OtherInput.CraftingState state = ChestManager.Instance.chests[num2].GetComponentInChildren<ChestInteract>().state;
                OtherInput.Instance.ToggleInventory(state);
            }

            // Skip original
            return false;
        }
    }
}
