using System;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using Harmony;
using RimWorld;
using NinjaMode.Harmony;

namespace NinjaMode
{
    [StaticConstructorOnStartup]
    static class NinjaModeInit
    {
        static NinjaModeInit()
        {
            var harmony = HarmonyInstance.Create("com.hg-dev.rimworld.mod.ninjamode");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            foreach (var x in DefDatabase<RoomRoleDef>.AllDefsListForReading)
            {
                Log.Message(x.label);
                Log.Message(x.defName);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_DraftController), "set_Drafted")]
    public static class Pawn_DraftController_Drafted_Patch
    {
        static public Dictionary<int, int> cooldowns = new Dictionary<int, int>();

        static void Postfix(Pawn_DraftController __instance)
        {
            if (__instance.pawn.story == null
                || __instance.pawn.story.adulthood == null
                || __instance.pawn.story.adulthood.identifier != "NinjaAssassin4")
            {
                // Do nothing
            }
            else if (__instance.pawn.mindState.inspirationHandler != null
                && __instance.pawn.Drafted == true)
            {
                __instance.pawn.mindState.inspirationHandler.TryStartInspiration(
                    //InspirationDefOf.InspiredTrade //This worked
                    InspirationDefOfNinjaMode.InspiredNinja
                    );
                __instance.pawn.needs.rest.
            }
        }

        /*[HarmonyPostfix]
        public static void TriggerNinjaInspiration(Pawn_DraftController __instance)
        {
            if (__instance.Drafted)
            {
                Messages.Message(
                    "NinjaMode_TestMessage".Translate(new object[] { __instance.pawn.NameStringShort }),
                    MessageTypeDefOf.PositiveEvent);
                //
            }
        }*/
    }
}
