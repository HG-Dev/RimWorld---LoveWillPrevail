using System;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using Harmony;
using RimWorld;
using UnityEngine;


namespace Suffixware.LoveWillPrevail.Harmony
{
    [HarmonyPatch(typeof(DamageWorker_AddInjury), "ApplyDamageToPart")]
    internal static class DamageWorker_AddInjury_Patch
    {
        public static void Postfix()
        {
            Log.Message("LWP: Oof");
        }
    }
}
