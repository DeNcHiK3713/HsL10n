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
    [HarmonyPatch(typeof(DbfLocValue), nameof(DbfLocValue.GetString), typeof(Locale))]
    public static class DbfLocValue_GetString
    {
        public static bool Prefix(bool ___m_stripped, string ___m_currentLocaleValue, int ___m_locId, ref string __result)
        {
            if (___m_stripped)
            {
                __result = ___m_currentLocaleValue;
                return false;
            }

            if (___m_locId > 0 && Resources.DbfStrings.TryGetValue(___m_locId, out var value))
            {
                __result = value;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(DbfLocValue), nameof(DbfLocValue.StripUnusedLocales))]
    public static class DbfLocValue_StripUnusedLocales
    {
        public static bool Prefix(ref bool ___m_stripped, ref string ___m_currentLocaleValue, int ___m_locId, ref string[] ___m_locValues)
        {
            if (___m_stripped)
            {
                return false;
            }

            if (___m_locId > 0 && Resources.DbfStrings.TryGetValue(___m_locId, out var value))
            {
                ___m_stripped = true;
                ___m_currentLocaleValue = value;
                ___m_locValues = null;

                return false;
            }

            return true;
        }
    }
}
