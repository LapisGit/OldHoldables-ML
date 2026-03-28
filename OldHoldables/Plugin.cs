using MelonLoader;
using GorillaNetworking;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using System;
using MelonLoader.Utils;
using OldHoldables;

[assembly: MelonInfo(typeof(OldHoldables.Plugin), PluginInfo.Name, PluginInfo.Version, PluginInfo.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
namespace OldHoldables
{
    public class Plugin : MelonMod
    {
        public static new MelonPreferences_Category Config;
        public static MelonPreferences_Entry<bool> disableDropping;
        
        public override void OnInitializeMelon()
        {
            Config = MelonPreferences.CreateCategory("OldHoldables");

            string configPath = Path.Combine(MelonEnvironment.UserDataDirectory, "OldHoldables.cfg");
            Config.SetFilePath(configPath);
            Config.LoadFromFile();
            
            disableDropping = Config.CreateEntry("disableDropping", false, "Disable Dropping", "Turn off manual dropping altogether. Not recommended, but may be needed for Index controllers");
            
            GameObject root = new GameObject(PluginInfo.Name);
            UnityEngine.Object.DontDestroyOnLoad(root);
            root.AddComponent<OHManager>();
        }
    }
}
