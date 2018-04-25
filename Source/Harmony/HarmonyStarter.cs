﻿using System;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using Harmony;
using RimWorld;
using UnityEngine;

namespace Suffixware
{
	public static class HarmonyStarter
	{
        private static HarmonyInstance harmony = null;

        static internal HarmonyInstance instance
        {
            get
            {
                if (harmony == null)
                    harmony = HarmonyInstance.Create("com.hg-dev.rimworld.mod.lovewillprevail");
                return harmony;
            }
        }

        public static void RunPatches()
        {
            // Remove the remark on the following to debug all auto patches.
            //HarmonyInstance.DEBUG = true;
            instance.PatchAll(Assembly.GetExecutingAssembly());
            // Keep the following remarked to also debug manual patches.
            //HarmonyInstance.DEBUG = false;
        }
    }
}
