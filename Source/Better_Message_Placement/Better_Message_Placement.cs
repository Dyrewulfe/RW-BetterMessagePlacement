using HarmonyLib;
using Verse;

<<<<<<< HEAD
namespace Better_Message_Placement;

[StaticConstructorOnStartup]
public class Better_Message_Placement
{
    private static readonly Harmony harmony = new Harmony("rimworld.dyrewulfe.bettermessageplacement");

    static Better_Message_Placement()
    {
        harmony.PatchAll();
=======
namespace Better_Message_Placement
{
    [StaticConstructorOnStartup]
    public class Better_Message_Placement
    {
        private static readonly Harmony harmony = new Harmony("rimworld.dyrewulfe.bettermessageplacement");

        static Better_Message_Placement()
        {
            harmony.PatchAll();
        }
>>>>>>> eb44ffc87f53d393dea8a09952335573473e572e
    }
}