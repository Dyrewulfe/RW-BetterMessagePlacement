using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Better_Message_Placement;

[HarmonyPatch(typeof(Messages), "MessagesDoGUI")]
public class Messages_MessagesDoGUI_Patch
{
    private static readonly FieldInfo vector2_y = AccessTools.Field(typeof(Vector2), "y");

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var prev = instructions.First();
        var patched = false;

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
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(Messages_MessagesDoGUI_Patch), nameof(YOffsetAdjustment)));
                yield return new CodeInstruction(OpCodes.Add);
            }

            prev = code;
        }
    }

    private static int YOffsetAdjustment()
    {
        var offset = (int)Messages.MessagesTopLeftStandard.y;
        if (Find.CurrentMap == null)
        {
            return offset;
        }

        var scale = Find.ColonistBar.Scale;
        var rows = 3;
        if (scale > 0.42f)
        {
            rows = 2;
        }

        if (scale > 0.58f)
        {
            rows = 1;
        }

        offset += (int)(Find.ColonistBar.Size.y + (24f * scale)) * rows;

        return offset;
    }
}