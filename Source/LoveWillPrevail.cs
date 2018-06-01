using System;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using Harmony;
using RimWorld;
using UnityEngine;
using Suffixware;

namespace Suffixware.LoveWillPrevail
{
    public class ModController : Mod
    {
        //public static Settings
        public static int MIN_TRIGGER_OPINION = 0;

        public ModController(ModContentPack content) : base(content)
        {
            // Apply Harmony patches
            HarmonyStarter.RunPatches();
        }

        public override string SettingsCategory()
        {
            return "Love Will Prevail";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            //TODO
            base.DoSettingsWindowContents(inRect);
        }
    }
}
