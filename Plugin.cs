using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using HarmonyLib;

using IPA;

using UnityEngine;

using IPALogger = IPA.Logging.Logger;


namespace HitSoundChanger {


    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {

        public static BS_Utils.Utilities.Config Settings = new BS_Utils.Utilities.Config("HitSoundChanger/HitSoundChanger");
        public static IPALogger Logger;

        public static List<HitSoundCollection> hitSounds = new List<HitSoundCollection>();
        public static HitSoundCollection currentHitSound;

        public static List<AudioClip> originalShortSounds;
        public static List<AudioClip> originalLongSounds;
        public static List<AudioClip> originalBadSounds;


        [OnStart]
        public void OnApplicationStart() {
            var harmony = new Harmony("com.kyle1413.BeatSaber.HitSoundChanger");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            UI.HitSoundChangerUI.OnLoad();
            SharedCoroutineStarter.instance.StartCoroutine(LoadAudio());
        }

        [Init]
        public void Init(IPALogger pluginLogger) {
            Logger = pluginLogger;
        }

        public IEnumerator LoadAudio() {

            Logger.Notice("Attempting to load Audio files");
            var folderPath = Environment.CurrentDirectory + "/UserData/HitSoundChanger";
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            var directories = Directory.GetDirectories(folderPath);
            hitSounds.Add(new HitSoundCollection { name = "Default", folderPath = "Default" });
            foreach (var folder in directories) {
                HitSoundCollection newSounds = new HitSoundCollection(folder);
                yield return newSounds.LoadSounds();
                hitSounds.Add(newSounds);
            }
            string lastSound = Settings.GetString("HitSoundChanger", "Last Selected Sound", "Default", true);
            HitSoundCollection lastSounds = hitSounds.FirstOrDefault(x => x.folderPath == lastSound);
            if (lastSounds == null) {
                lastSounds = hitSounds[0];
            }
            currentHitSound = lastSounds;
        }
    }
}
