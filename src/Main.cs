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
    [EnableReloading]
    public static class Main
    {
        internal static Harmony HarmonyInstance;
        private static readonly List<GameObject> ModObjects = new List<GameObject>();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            PluginLogger.Init(modEntry.Path);
            PluginLogger.Log("Initializing...");

            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;
            modEntry.OnUnload = OnUnload;

            PluginLogger.Log("Loaded successfully.");

            return true;
        }

        public static bool OnUnload(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance?.UnpatchAll(HarmonyInstance.Id);

            foreach (GameObject go in ModObjects)
            {
                if (go != null)
                    Object.Destroy(go);
            }
            ModObjects.Clear();

            PluginLogger.Log("Unloaded mod.");
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                if (!ModObjects.Any(go => go != null))
                {
                    RegisterGameObject<DebugOverlayBehaviour>();
                }
            }
            else
            {
                foreach (GameObject go in ModObjects)
                {
                    if (go != null)
                    {
                        Object.Destroy(go);
                    }
                }
                ModObjects.Clear();
            }

            return true;
        }

        private static void RegisterGameObject<T>() where T : Component
        {
            GameObject gameObject = new GameObject("Purps :: " + typeof(T).Name);
            Object.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<T>();

            ModObjects.Add(gameObject);
        }
    }
}
