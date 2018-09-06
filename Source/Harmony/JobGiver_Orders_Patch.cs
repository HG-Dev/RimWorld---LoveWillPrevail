using System.Collections.Generic;
using Verse;
using Verse.AI;
using Harmony;
using RimWorld;
using UnityEngine;


namespace Suffixware.LoveWillPrevail.Harmony
{
    [HarmonyPatch(typeof(JobGiver_Orders), "TryGiveJob")]
    internal static class JobGiver_Orders_TryGiveJob_Patch
    {
        [HarmonyPostfix]
        public static void InspireIfMoveNearTrigger(ref Job __result, Pawn pawn)
        {
            if (__result != null && pawn != null)
            {
                if (pawn.Drafted)
                {
                    Log.Message("LWP: Orders issued");
                    // Inspire the pawn if the pawn is moving near an injured Trigger
                    // いわゆる駆けつけシーン（言わない）
                }
            }
        }
    }
}