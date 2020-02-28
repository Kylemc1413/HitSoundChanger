using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPA;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using UnityEngine.Networking;
namespace HitSoundChanger
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static BS_Utils.Utilities.Config Settings = new BS_Utils.Utilities.Config("HitSoundChanger/HitSoundChanger");

        public static List<HitSoundCollection> hitSounds = new List<HitSoundCollection>();
        public static HitSoundCollection currentHitSound { get; internal set; }

        public static List<AudioClip> originalShortSounds;
        public static List<AudioClip> originalLongSounds;
        public static List<AudioClip> originalBadSounds;
        [OnStart]
        public void OnApplicationStart()
        {
            var harmony = new Harmony("com.kyle1413.BeatSaber.HitSoundChanger");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            HitSoundChanger.UI.HitSoundChangerUI.OnLoad();
            SharedCoroutineStarter.instance.StartCoroutine(LoadAudio());
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        [Init]
        public void Init(object thisIsNull, IPALogger pluginLogger)
        {

            Utilities.Logging.Log = pluginLogger;
        }

        public IEnumerator LoadAudio()
        {
         
            Utilities.Logging.Log.Notice("Attempting to load Audio files");
            var folderPath = Environment.CurrentDirectory + "/UserData/HitSoundChanger";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var directories = Directory.GetDirectories(folderPath);
            hitSounds.Add(new HitSoundCollection { name = "Default", folderPath = "Default" });
            foreach(var folder in directories)
            {
                HitSoundCollection newSounds = new HitSoundCollection(folder);
                yield return newSounds.LoadSounds();
                hitSounds.Add(newSounds);
            }
            string lastSound = Settings.GetString("HitSoundChanger", "Last Selected Sound", "Default", true);
            HitSoundCollection lastSounds = hitSounds.FirstOrDefault(x => x.folderPath == lastSound);
            if (lastSounds == null)
                lastSounds = hitSounds[0];
            currentHitSound = lastSounds;


        }
      
        public void OnApplicationQuit()
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {

        }

    }
}
