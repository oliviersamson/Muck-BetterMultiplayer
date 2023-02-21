using HarmonyLib;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine.UI;

namespace BetterMultiplayer.UiSettingsPatch
{
    [HarmonyPatch(typeof(UiSettings), "AddSettings")]
    static class AddSettingsTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {       
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            //Define label for jump
            Label label = generator.DefineLabel();

            // Match instruction loading local variable 2 before call to get_onClick()
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Ldloc_2));

            // Load current UiSettings instance (argument at index 0) and newly created Button component (local variable at index 2)
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0));
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_2));

            // Emit call to delagate, consuming current UiSettings instance and Button component, to add buttons to LobbyData Buttons dictionary
            codeMatcher = codeMatcher.InsertAndAdvance(Transpilers.EmitDelegate<Action<UiSettings, Button>>(
                (instance, button) => {

                    LobbyData.Buttons[instance.name].Add(button);
                }));

            // Insert instruction jumping to label if above delegate returned false
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Br, label));

            Plugin.Log.LogDebug("Skipped initial Settings Button callback setup");

            // Match to instruction loading current instance after the delegate definition
            //codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Callvirt));
            codeMatcher = codeMatcher.MatchForward(true,
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(OpCodes.Ldarg_0));

            // Set the previously defined label to this instruction
            codeMatcher.SetInstructionAndAdvance(codeMatcher.InstructionAt(0).WithLabels(label));

            return codeMatcher.InstructionEnumeration();
        }
    }
}
