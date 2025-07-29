using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using Kingmaker.Blueprints;
using System.Linq;
using System.Collections.Generic;
using Purps.RogueTrader.Logging;
using Purps.RogueTrader.Behaviours;
using System.Reflection;

namespace Purps.RogueTrader
{
#if DEBUG
    [EnableReloading]
#endif
    public static class Main
    {
        internal static Harmony harmonyInstance;

        public static Settings settings;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

            PluginLogger.Init(modEntry.Path);
            PluginLogger.Log("Initializing...");

            harmonyInstance = new Harmony(modEntry.Info.Id);
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;
            modEntry.OnUnload = OnUnload;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnGUI = OnGUI;

            PluginLogger.Log("Loaded successfully.");

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry entry)
        {
            SettingsUI.OnGUI();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static bool OnUnload(UnityModManager.ModEntry modEntry)
        {
            harmonyInstance?.UnpatchAll(harmonyInstance.Id);

            PluginLogger.Log("Unloaded mod.");
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            return true;
        }

        public static void RegisterGameObject<T>() where T : Component
        {
            GameObject gameObject = new GameObject("Purps :: " + typeof(T).Name);
            Object.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<T>();
        }

        public static void UnregisterGameObject<T>() where T : Component
        {
            string objectName = "Purps :: " + typeof(T).Name;
            GameObject gameObject = GameObject.Find(objectName);

            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
