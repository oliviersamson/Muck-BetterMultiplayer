using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


namespace BetterMultiplayer.ClientHandlePatch
{
    [HarmonyPatch(typeof(ClientHandle), "UpdateChest")]
    class UpdateChestTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            // Match end of method
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ret));
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ret));

            // Load chestId, cellId, itemId and amount (local variables at index 0 to 3) into top of stack
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0));
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_1));
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_2));
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3));

            codeMatcher = codeMatcher.InsertAndAdvance(Transpilers.EmitDelegate<Action<int, int, int, int>>(
                (chestId, cellId, itemId, amount) => {

                    Plugin.Log.LogInfo($"chestId = {chestId}, cellId = {cellId}, itemId = {itemId}, amount = {amount}");

                    if (OtherInput.Instance.currentChest == null)
                    {
                        return;
                    }

                    Plugin.Log.LogInfo($"craftingState = {OtherInput.Instance.craftingState}");

                    if (OtherInput.Instance.currentChest.id == chestId)
                    {

                        InventoryItem inventoryItem = null;

                        if (itemId != -1)
                        {
                            inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
                            inventoryItem.Copy(ItemManager.Instance.allItems[itemId], amount);
                        }

                        switch (OtherInput.Instance.craftingState)
                        {
                            case OtherInput.CraftingState.Chest:

                                ((ChestUI)OtherInput.Instance.chest).cells[cellId].currentItem = inventoryItem;
                                ((ChestUI)OtherInput.Instance.chest).cells[cellId].UpdateCell();

                                break;

                            case OtherInput.CraftingState.Cauldron:

                                ((CauldronUI)OtherInput.Instance.cauldron).synchedCells[cellId].currentItem = inventoryItem;
                                ((CauldronUI)OtherInput.Instance.cauldron).synchedCells[cellId].UpdateCell();

                                break;

                            case OtherInput.CraftingState.Furnace:

                                ((FurnaceUI)OtherInput.Instance.cauldron).synchedCells[cellId].currentItem = inventoryItem;
                                ((FurnaceUI)OtherInput.Instance.cauldron).synchedCells[cellId].UpdateCell();

                                break;
                        }
                    }
                }));

            return codeMatcher.InstructionEnumeration();
        }
    }
}
