using System.Collections.Generic;
using Verse;
using Verse.AI;
using Harmony;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Suffixware.Util;

namespace Suffixware.LoveWillPrevail.Harmony
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    class FloatMenuMakerMap_AddDraftedOrders_Patch
    {

        [HarmonyPostfix]
        public static void InspireIfOrdersNearTrigger(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            foreach (var inspiration in DefDatabase<InspirationDef>.AllDefsListForReading)
            {
                Log.Message(inspiration.defName);
            }

            if (pawn != null && pawn.mindState.inspirationHandler != null)
            {
                IntVec3 clickCell = IntVec3.FromVector3(clickPos);
                bool trigger = false;
                // Get pawns near clickCell and determine if any of them can be a Trigger
                var socialEntries = SocialInfoUtility.GetSocialEntries(pawn);
                foreach (var entry in socialEntries)
                {
                    if (entry.opinionOfOtherPawn > ModController.MIN_TRIGGER_OPINION
                        && GenSight.LineOfSight(clickCell, entry.otherPawn.Position, pawn.Map, true))
                    {
                        Log.Message("Found loved ally near destination");
                        var ally = entry.otherPawn;
                        
                        //Check to see if that pawn is in danger and in sight
                        //Danger = targeted or downed
                        //Consider having targeted require a higher opinion to trigger
                        //Quick trigger
                        trigger = (
                            ally.mindState.meleeThreat != null
                            || ally.IsBurning()
                            || ally.Downed
                            || ally.IsFighting()
                        );
                        Log.Message(string.Format("Ally data: {0} {1} {2} {3}", ally.mindState.meleeThreat != null, ally.IsBurning(), ally.Downed, ally.IsFighting()));
                        if (!trigger)
                        {
                            //Check battle log for scrapes
                            foreach (var battleIncident in Find.BattleLog.RawEntries)
                            {
                                trigger = (battleIncident.Age < 1000 && battleIncident.Concerns(ally));
                                if (trigger)
                                {
                                    Log.Message(battleIncident.ToGameStringFromPOV(ally));
                                    break;
                                }
                            }
                        }
                    }
                }

                if (trigger)
                {
                    Log.Message("LWP: TRIGGERING + " + pawn.NameStringShort);
                    if (pawn.Inspired && pawn.InspirationDef != InspirationDefOfLWP.InspiredHeroic)
                    {
                        pawn.mindState.inspirationHandler.EndInspiration(pawn.Inspiration);
                    }
                    var success = pawn.mindState.inspirationHandler.TryStartInspiration(
                        InspirationDefOfLWP.InspiredHeroic
                        );
                    if (!success) Log.Message("LWP: Triggering failed.");
                }
            }
        }
    }
}
