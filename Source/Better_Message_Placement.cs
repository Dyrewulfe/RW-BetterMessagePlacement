using Harmony;
using RimWorld;
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

            static HarmonyInstance harmony = HarmonyInstance.Create("rimworld.dyrewulfe.bettermessageplacement");

            static HarmonyPatches()
            {
                HarmonyInstance.DEBUG = true;
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
                        if (!patched)
                        {
                            if (prev.opcode == OpCodes.Ldfld && prev.operand == vector2_y)
                            {
                                patched = true;
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Messages_MessagesDoGUI_Patch), nameof(YOffsetAdjustment)));
                                yield return new CodeInstruction(OpCodes.Add);
                            }
                            prev = code;
                        }
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

                static FieldInfo vector2_y = AccessTools.Field(typeof(Vector2), "y");
            }
        }
    }
}
