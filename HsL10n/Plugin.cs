using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HsL10n
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            var formatter = new BinaryFormatter();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BepInEx\plugins\");
            foreach (var file in Directory.EnumerateFiles(path, "*.bin", SearchOption.AllDirectories))
            {
                if (file.EndsWith("gamestrings.bin", StringComparison.OrdinalIgnoreCase))
                {
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    Resources.GameStrings = (Dictionary<string, string>)formatter.Deserialize(fs);
                }
                else if (file.EndsWith("dbfstrings.bin", StringComparison.OrdinalIgnoreCase))
                {
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    Resources.DbfStrings = (Dictionary<int, string>)formatter.Deserialize(fs);
                }

                if (Resources.GameStrings.Count > 0 && Resources.DbfStrings.Count > 0)
                {
                    break;
                }
            }

            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            var disabled = Config.Bind("Localization", "Disabled", false);

            disabled.SettingChanged += (_, _) =>
            {
                if (disabled.Value)
                {
                    harmony.UnpatchSelf();
                }
                else
                {
                    harmony.PatchAll();
                }

                // TODO: Reload stripped DbfLocValue
            };

            if (!disabled.Value)
            {
                harmony.PatchAll();
            }

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
