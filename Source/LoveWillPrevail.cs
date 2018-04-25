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
    /*
    [StaticConstructorOnStartup]
    static class LoveWillPrevail
    {
        static LoveWillPrevail()
        {
            var harmony = HarmonyInstance.Create("com.hg-dev.rimworld.mod.lovewillprevail");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        
    }*/

    public class Controller : Mod
    {
        //public static Settings

        public Controller(ModContentPack content) : base(content)
        {
            // Apply Harmony patches
            HarmonyStarter.RunPatches();
            var harmony = HarmonyInstance.Create("hg-dev.rimworld.mod.lovewillprevail");
            HarmonyInstance.DEBUG = true;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            HarmonyInstance.DEBUG = false;

            Log.Message("Love Will Prevail initialized.");
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
