using HarmonyLib;
using System;

namespace UnityEngine
{
    public static class ExtendServerSend
    {
        public static void OpenChest(int fromClient, int chestId, bool use, bool stillInUse)
        {
            using (Packet packet = new Packet(25))
            {
                packet.Write(fromClient);
                packet.Write(chestId);
                packet.Write(use);
                packet.Write(stillInUse);
                AccessTools.Method(typeof(ServerSend), "SendTCPDataToAll", new Type[] { typeof(Packet) }).Invoke(null, new object[] { packet });
            }
        }
    }
}

