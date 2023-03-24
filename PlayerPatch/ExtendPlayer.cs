using System.Runtime.CompilerServices;

namespace UnityEngine
{
    public static class ExtendPlayer
    {
        private static readonly ConditionalWeakTable<Player, ChestId> PlayersChestId = new ConditionalWeakTable<Player, ChestId>();

        private class ChestId
        {
            public int Value { get; set; } = -1;
        }

        public static void SetChestUnused(this Player player)
        {
            PlayersChestId.GetOrCreateValue(player).Value = -1;
        }

        public static bool IsChestUsed(this Player player, int chestId)
        {
            return PlayersChestId.GetOrCreateValue(player).Value == chestId;
        }

        public static void UseChest(this Player player, int chestId)
        {
            PlayersChestId.GetOrCreateValue(player).Value = chestId;
        }
    }
}