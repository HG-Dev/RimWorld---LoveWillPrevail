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
            
            if (pawn != null && pawn.mindState.inspirationHandler != null)
            {
                IntVec3 clickCell = IntVec3.FromVector3(clickPos);
                bool trigger = IsNinja(pawn);
                // Get pawns near clickCell and determine if any of them can be a Trigger
                var socialEntries = SocialInfoUtility.GetSocialEntries(pawn);
                Pawn ally = null;
                foreach (var entry in socialEntries)
                {
                    if (entry.opinionOfOtherPawn > ModController.MIN_TRIGGER_OPINION
                        && (trigger || (clickCell - entry.otherPawn.Position).LengthHorizontalSquared < 36))
                    {
                        ally = entry.otherPawn;
                        //Check to see if that pawn is in danger and nearby clicked spot
                        //Quick trigger
                        trigger = (
                            ally.mindState.meleeThreat != null
                            || ally.IsBurning()
                            || ally.Downed
                            || ally.IsFighting()
                        );

                        if (trigger) break;
                        /*if (!trigger && IsNinja(pawn)) //If no obvious threats, but ally is  fighting and pawn is ninja
                        if (!trigger)
                        {
                            //Check battle log for scrapes
                            foreach (var battleIncident in Find.BattleLog.RawEntries)
                            {
                                trigger = (battleIncident.Age < 1000 && battleIncident.Concerns(ally));
                                if (trigger) break;
                            }
                        }*/
                    }
                }
                if (trigger)
                {
                    Log.Message(string.Format("Triggering {0} for {1} ", pawn.Name,  ally.Name));
                    if (pawn.mindState.inspirationHandler.TryStartInspiration(
                        InspirationDefOfLWP.InspiredHeroic
                        ))
                    {
                        var whichDiff = IsNinja(pawn) ? HediffDefOfLWP.LWP_StressHardened : HediffDefOfLWP.LWP_Stress;
                        var hediff = HediffMaker.MakeHediff(whichDiff, pawn, null);
                        pawn.health.AddHediff(hediff, null, null);
                    }
                }
            }

            bool IsNinja(Pawn myPawn)
            {
                if (myPawn.story == null
                    || myPawn.story.adulthood == null
                    || !myPawn.story.adulthood.identifier.ToLower().Contains("ninja"))
                    return false;
                return true;
            }

            Hediff GetStressHediff(Pawn myPawn)
            {
                return myPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOfLWP.LWP_Stress)
                    ?? myPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOfLWP.LWP_StressHardened);
            }
        }
    }
}
