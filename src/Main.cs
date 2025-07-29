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
        private static readonly List<GameObject> modObjects = new List<GameObject>();

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

            foreach (GameObject go in modObjects)
            {
                if (go != null)
                    Object.Destroy(go);
            }
            modObjects.Clear();

            PluginLogger.Log("Unloaded mod.");
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                if (!modObjects.Any(go => go != null))
                {
                    RegisterGameObject<OverlayBehaviour>();
                }
            }
            else
            {
                foreach (GameObject go in modObjects)
                {
                    if (go != null)
                    {
                        Object.Destroy(go);
                    }
                }
                modObjects.Clear();
            }

            return true;
        }

        private static void RegisterGameObject<T>() where T : Component
        {
            GameObject gameObject = new GameObject("Purps :: " + typeof(T).Name);
            Object.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<T>();

            modObjects.Add(gameObject);
        }
    }
}
