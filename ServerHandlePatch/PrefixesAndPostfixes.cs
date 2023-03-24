using HarmonyLib;
using UnityEngine;

namespace BetterMultiplayer.ServerHandlePatch
{
    static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(ServerHandle), "RequestChest")]
        [HarmonyPrefix]
        static bool RequestChestPrefix(int fromClient, Packet packet)
        {
            if (Server.clients[fromClient].player == null)
            {
                return false;
            }
            int num = packet.ReadInt(true);
            bool flag = packet.ReadBool(true);
            if (!ChestManager.Instance.chests.ContainsKey(num))
            {
                return false;
            }
            if (flag)
            {
                Server.clients[fromClient].player.UseChest(num);

                ChestManager.Instance.UseChest(num, true);
                ExtendServerSend.OpenChest(fromClient, num, true, true);
                ChestManager.Instance.chests[num].GetComponent<ChestInteract>().ServerExecute(fromClient);
            }
            else
            {
                Server.clients[fromClient].player.SetChestUnused();

                bool isStillUsed = false;
                foreach (Client client in Server.clients.Values)
                {
                    if (client.player != null && client.player.IsChestUsed(num))
                    {
                        isStillUsed = true;
                    }
                }

                ChestManager.Instance.UseChest(num, isStillUsed);
                ExtendServerSend.OpenChest(fromClient, num, false, isStillUsed);
            }

            return false;
        }
    }
}
