using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;


namespace Better_Message_Placement
{
    public class Better_Message_Placement
    {
        [StaticConstructorOnStartup]
        static class HarmonyPatches
        {

            static readonly Harmony harmony = new Harmony("rimworld.dyrewulfe.bettermessageplacement");

            static HarmonyPatches()
            {
                harmony.PatchAll();
            }

            [HarmonyPatch(typeof(Messages), "MessagesDoGUI")]
            class Messages_MessagesDoGUI_Patch
            {
                static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instructions)
                {
                    CodeInstruction prev = instructions.First();
                    bool patched = false;

                    foreach (var code in instructions)
                    {
                        yield return code;
                        if (patched)
                        {
                            continue;
                        }
                        if (prev.opcode == OpCodes.Ldfld && (FieldInfo)prev.operand == vector2_y)
                        {
                            patched = true;
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Messages_MessagesDoGUI_Patch), nameof(YOffsetAdjustment)));
                            yield return new CodeInstruction(OpCodes.Add);
                        }
                        prev = code;
                    }
                }

                static int YOffsetAdjustment()
                {
                    int offset = (int)Messages.MessagesTopLeftStandard.y;
                    if (Find.CurrentMap != null)
                    {
                        float scale = Find.ColonistBar.Scale;
                        int rows = 3;
                        if (scale > 0.42f)
                        {
                            rows = 2;
                        }
                        if (scale > 0.58f)
                        {
                            rows = 1;
                        }
                        offset += (int)(Find.ColonistBar.Size.y + (24f * scale)) * rows;
                    }
                    return offset;
                }

                static readonly FieldInfo vector2_y = AccessTools.Field(typeof(Vector2), "y");
            }
        }
    }
}
