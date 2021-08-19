using HarmonyLib;
using Verse;

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
    }
}