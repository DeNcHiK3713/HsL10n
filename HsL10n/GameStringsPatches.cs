using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HsL10n
{
    [HarmonyPatch(typeof(GameStrings), "Find")]
    public static class GameStrings_Find
    {
        public static bool Prefix(string key, ref string __result)
        {
            if (key == null)
            {
                __result = null;
                return false;
            }

            if (Resources.GameStrings.TryGetValue(key, out var value))
            {
                __result = value;
                return false;
            }

            return true;
        }
    }
}
